using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.scripts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseLessonRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseLessonRepository> logger
) : ICourseLessonRepository
{
    public async Task<int> AddCourseLesson(CourseLesson courseLesson)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var maxOrder = await dbContext.CourseLessons
                .Where(c => c.CourseSectionId == courseLesson.CourseSectionId)
                .MaxAsync(c => (int?)c.Order) ?? 0;

            courseLesson.Order = maxOrder + 1;
            dbContext.CourseLessons.Add(courseLesson);

            await dbContext.SaveChangesAsync();
            await dbContext.UpdateCourseLessonsOrder(lessonId: courseLesson.CourseLessonId);
            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return courseLesson.CourseLessonId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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
            .Where(cs => cs.CourseSectionId == sectionId).Include(courseLesson => courseLesson.CourseLessonResources)
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

    public async Task UpdateCourseLessonsAsync(List<CourseLesson> updatedLessons)
    {
        dbContext.CourseLessons.UpdateRange(updatedLessons);
        await dbContext.SaveChangesAsync();
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
}