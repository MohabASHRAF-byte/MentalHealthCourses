using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ArgumentException = System.ArgumentException;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseInteractionsRepository(
    MentalHealthDbContext dbContext
) : ICourseInteractionsRepository
{
    public async Task Enroll(int courseId, int userId)
    {
        var isCourseFree = await dbContext.Courses
            .AnyAsync(c => c.CourseId == courseId && c.IsFree == true);

        if (!isCourseFree)
        {
            throw new ArgumentException("Course is not free");
        }

        var isOwned = await dbContext.CourseProgresses
            .AnyAsync(c => c.CourseId == courseId && c.SystemUserId == userId);

        if (isOwned)
        {
            throw new ArgumentException("Course is already owned");
        }

        var courseProgress = new CourseProgress()
        {
            CourseId = courseId,
            SystemUserId = userId,
            LastChange = DateTime.Now,
            LastLessonIdx = 0
        };
        await dbContext.AddAsync(courseProgress);
        await dbContext.SaveChangesAsync();
    }


    public async Task CompleteLessonAsync(int courseId, int lessonId, int userId)
    {
        // Fetch lesson order and validate if lesson exists
        var lessonOrder = await dbContext.CourseLessons
            .Where(cl => cl.CourseLessonId == lessonId && cl.CourseSection.CourseId == courseId)
            .Select(cl => cl.OrderOnCourse)
            .FirstOrDefaultAsync();

        if (lessonOrder == 0)
        {
            throw new ArgumentException("Invalid lesson ID or the lesson does not belong to the specified course");
        }

        // Fetch course progress
        var courseProgress = await dbContext.CourseProgresses
            .Where(cp => cp.CourseId == courseId && cp.SystemUserId == userId)
            .FirstOrDefaultAsync();

        if (courseProgress == null)
        {
            throw new ArgumentException("User is not enrolled in this course");
        }

        if (courseProgress.LastLessonIdx >= lessonOrder)
        {
            return;
        }

        if (courseProgress.LastLessonIdx + 1 != lessonOrder)
        {
            throw new ArgumentException("Complete the previous lesson before proceeding");
        }

        // Update progress
        courseProgress.LastLessonIdx = lessonOrder;
        courseProgress.LastChange = DateTime.Now;
        await dbContext.SaveChangesAsync();
    }

    public async Task<CourseLessonDto> GetLessonAsync(int lessonId)
    {
        var lesson = await dbContext.CourseLessons
            .Include(cl => cl.CourseLessonResources)
            .FirstOrDefaultAsync(cl => cl.CourseLessonId == lessonId);
        if (lesson == null)
        {
            throw new ArgumentException("Lesson not found.");
        }

        lesson.views += 1;
        await dbContext.SaveChangesAsync();
        return new CourseLessonDto
        {
            CourseLessonId = lesson.CourseLessonId,
            LessonName = lesson.LessonName,
            Order = lesson.Order,
            CourseLessonResources = lesson.CourseLessonResources?
                .Select(resource => new CourseResourceDto
                {
                    CourseLessonResourceId = resource.CourseLessonResourceId,
                    Title = resource.Title,
                    ItemOrder = resource.ItemOrder,
                    Url = resource.Url,
                    ContentType = resource.ContentType
                }).ToList() ?? []
        };
    }
}