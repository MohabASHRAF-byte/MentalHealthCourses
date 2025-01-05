using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.scripts;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseLessonRepository(
    MentalHealthDbContext dbContext
) : ICourseLessonRepository
{
    public async Task<int> AddCourseLesson(CourseLesson courseLesson)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Retrieve the maximum order within the section
            var maxOrder = await dbContext.CourseLessons
                .Where(c => c.CourseSectionId == courseLesson.CourseSectionId)
                .MaxAsync(c => (int?)c.Order) ?? 0;

            // Set the order for the new lesson
            courseLesson.Order = maxOrder + 1;
            dbContext.CourseLessons.Add(courseLesson);

            // Retrieve the associated course
            var course = await dbContext.Courses
                .Where(c => c.CourseId == courseLesson.courseId)
                .FirstOrDefaultAsync();
            if (course == null)
            {
                throw new ResourceNotFound("course", courseLesson.courseId.ToString());
            }

            // Increment the lesson count for the course
            course.LessonsCount++;

            // Save changes for adding the lesson and updating the course
            await dbContext.SaveChangesAsync();

            // Update the order of lessons within the course if required
            await dbContext.UpdateCourseLessonsOrder(lessonId: courseLesson.CourseLessonId);

            // Commit the transaction
            await transaction.CommitAsync();
            return courseLesson.CourseLessonId;
        }
        catch (Exception ex)
        {
            // Rollback the transaction on error
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Error adding course lesson", ex);
        }
    }

    public async Task<bool> IsSectionOrderable(int courseId, int sectionId)
    {
        return !await dbContext.CourseProgresses
            .AnyAsync(p => p.CourseId == courseId);
    }


    public async Task<List<CourseLesson>> GetCourseLessons(int courseId, int sectionId)
    {
        var lessons = dbContext.CourseLessons
            .Where(cs => cs.CourseSectionId == sectionId
            );
        return await lessons.ToListAsync();
    }

    public async Task<List<CourseLessonViewDto>> GetCourseLessonsDto(int courseId, int sectionId)
    {
        // Fetch the lessons from the database
        var lessons = await dbContext.CourseLessons
            .Where(cs => cs.CourseSectionId == sectionId)
            .Include(courseLesson => courseLesson.CourseLessonResources)
            .OrderBy(cl => cl.Order)
            .ToListAsync();

        // Map CourseLesson entities to CourseLessonViewDto using AutoMapper
        var lessonDtos = lessons.Select(lesson => new CourseLessonViewDto
        {
            CourseLessonId = lesson.CourseLessonId,
            LessonName = lesson.LessonName,
            Order = lesson.Order,
            LessonResourcesCount =
                lesson.CourseLessonResources?.Count() ?? 0 // Assuming you want the count of resources
        }).ToList();

        return lessonDtos;
    }

    public async Task DeleteCourseLessonAsync(CourseLesson targetLesson)
    {
        dbContext.Remove(targetLesson);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseLessonsAsync(List<CourseLesson> updatedLessons, int courseId)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Update the lessons
            dbContext.CourseLessons.UpdateRange(updatedLessons);
            // Save changes
            await dbContext.SaveChangesAsync();
            // Update the course lessons order
            await dbContext.UpdateCourseLessonsOrder(courseId: courseId);

            await dbContext.SaveChangesAsync();
            // Commit the transaction
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            // Rollback the transaction in case of an error
            await transaction.RollbackAsync();
            throw new Exception("An error occurred while updating course lessons.", ex);
        }
    }


    public async Task<CourseLesson> GetCourseLessonByIdAsync(int id)
    {
        var courseLesson = await dbContext.CourseLessons
            .AsNoTracking()
            .Include(cls => cls.CourseLessonResources)
            .FirstOrDefaultAsync(c => c.CourseLessonId == id);
        if (courseLesson == null)
        {
            throw new ResourceNotFound(nameof(courseLesson), id.ToString());
        }

        return courseLesson;
    }

    public async Task<CourseLesson> GetCourseLessonByIdAsync(int courseId, int sectionId, int lessonId)
    {
        var lesson = await dbContext.CourseLessons
            .Where(
                cls =>
                    cls.CourseSectionId == sectionId
                    && cls.CourseLessonId == lessonId
                    && cls.courseId == courseId
            ).FirstOrDefaultAsync();
        if (lesson == null)
            throw new ResourceNotFound(nameof(lesson), lessonId.ToString());
        return lesson;
    }

    public async Task<CourseLesson> GetCourseFullLessonByIdAsync(int id)
    {
        var courseLesson = await dbContext.CourseLessons
            .Include(cl => cl.CourseLessonResources) // Include the resources
            .FirstOrDefaultAsync(c => c.CourseLessonId == id);

        if (courseLesson == null)
        {
            throw new ResourceNotFound(nameof(courseLesson), id.ToString());
        }

        if (courseLesson.CourseLessonResources != null)
        {
            courseLesson.CourseLessonResources = courseLesson.CourseLessonResources
                .OrderBy(r => r.ItemOrder) // Sort resources by 'ItemOrder'
                .ToList();
        }

        return courseLesson;
    }

    public async Task UpdateCourseLessonDataAsync(int lessonId, string name)
    {
        var courseLesson = new CourseLesson { CourseLessonId = lessonId };

        dbContext.CourseLessons.Attach(courseLesson);
        courseLesson.LessonName = name;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFound(nameof(CourseLesson), lessonId.ToString());
        }
    }

    public async Task RemoveLesson(int courseId, int sectionId, int lessonId)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Fetch the lesson to be removed
            var lesson = await dbContext.CourseLessons
                .Where(
                    cls =>
                        cls.CourseSectionId == sectionId
                        && cls.CourseLessonId == lessonId
                        && cls.courseId == courseId
                ).FirstOrDefaultAsync();

            if (lesson == null)
                throw new ResourceNotFound(nameof(lesson), lessonId.ToString());

            // Update course progresses
            var courseProgresses = await dbContext.CourseProgresses
                .Where(cp => cp.CourseId == courseId)
                .ToListAsync();

            foreach (var courseProgress in courseProgresses)
            {
                if (courseProgress.LastLessonIdx >= lesson.OrderOnCourse)
                    courseProgress.LastLessonIdx--;
            }

            // Adjust the order of lessons in the section
            var sectionLessons = await dbContext.CourseLessons
                .Where(l =>
                    l.courseId == courseId
                    && l.CourseSectionId == sectionId
                ).ToListAsync();

            foreach (var sectionLesson in sectionLessons)
            {
                if (sectionLesson.Order > lesson.Order)
                {
                    sectionLesson.Order--;
                }
            }

            // Remove the lesson
            dbContext.CourseLessons.Remove(lesson);

            // Save changes and update lesson order
            await dbContext.SaveChangesAsync();
            await dbContext.UpdateCourseLessonsOrder(courseId);
            await dbContext.SaveChangesAsync();
            // Commit transaction
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new TryAgain("Error occurred while removing the lesson.");
        }
    }
}