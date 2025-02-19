using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseInteractionsRepository(
    MentalHealthDbContext dbContext, 
    ILocalizationService localizationService
) : ICourseInteractionsRepository
{
    public async Task Enroll(int courseId, int userId)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var course = await dbContext.Courses
                .Where(c => c.CourseId == courseId && c.IsFree == true)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new BadHttpRequestException(
                    localizationService.GetMessage("CourseNotFree")
                );
            }

            course.EnrollmentsCount++;
            dbContext.Courses.Update(course);

            var isOwned = await dbContext.CourseProgresses
                .AnyAsync(c => c.CourseId == courseId && c.SystemUserId == userId);

            if (isOwned)
            {
                throw new BadHttpRequestException(
                    localizationService.GetMessage("CourseAlreadyOwned")
                );
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

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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
            throw new BadHttpRequestException(
                localizationService.GetMessage("InvalidLessonIdOrNotInCourse")
            );
        }

        // Fetch course progress
        var courseProgress = await dbContext.CourseProgresses
            .Where(cp => cp.CourseId == courseId && cp.SystemUserId == userId)
            .FirstOrDefaultAsync();

        if (courseProgress == null)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("UserNotEnrolledInCourse")
            );
        }

        if (courseProgress.LastLessonIdx >= lessonOrder)
        {
            return;
        }

        if (courseProgress.LastLessonIdx + 1 != lessonOrder)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("CompletePreviousLessonFirst")
            );
        }

        // Update progress
        courseProgress.LastLessonIdx = lessonOrder;
        courseProgress.LastChange = DateTime.Now;
        await dbContext.SaveChangesAsync();
    }

    public async Task<CourseLessonDto> GetLessonAsync(int courseId, int lessonId, int userId)
    {
        var lesson = await dbContext.CourseLessons
            .Include(cl => cl.CourseLessonResources)
            .FirstOrDefaultAsync(cl => cl.CourseLessonId == lessonId);

        if (lesson == null)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("LessonNotFound")
            );
        }

        if (lesson.courseId != courseId)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("LessonNotInCourse", "Lesson does not belong to this course.")
            );
        }

        var progress = await dbContext
            .CourseProgresses
            .Where(cp => cp.CourseId == lesson.courseId && cp.SystemUserId == userId)
            .FirstOrDefaultAsync();
        if (progress == null)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("NotEnrolledInCourse", "You are not enrolled in this course.")
            );
        }

        if (progress.LastLessonIdx + 1 < lesson.OrderOnCourse)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("WatchPreviousLessonsFirst", "You should watch previous lessons first.")
            );
        }

        lesson.views += 1;
        await dbContext.SaveChangesAsync();
        return new CourseLessonDto
        {
            CourseLessonId = lesson.CourseLessonId,
            LessonName = lesson.LessonName,
            Url = lesson.Url,
            LessonLengthInSeconds = lesson.LessonLengthInSeconds,
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

    public async Task<(int count, IEnumerable<CourseActivityDto> courses)> GetActiveCourseProgress(
        int userId,
        int pageNumber,
        int pageSize,
        string? courseName
    )
    {
        var coursesQuery = dbContext.CourseProgresses
            .Where(cp => cp.SystemUserId == userId)
            .OrderByDescending(cp => cp.LastChange)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(courseName))
        {
            coursesQuery = coursesQuery.Where(
                cp => cp.Course.Name.ToLower().Contains(courseName.ToLower()));
        }

        var totalCourses = await coursesQuery.CountAsync();

        var paginatedCourses = await coursesQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(cp => cp.Course)
            .ToListAsync();

        var result = paginatedCourses.Select(c =>
        {
            var totalLessons = c.Course.LessonsCount;

            return new CourseActivityDto()
            {
                CourseId = c.CourseId,
                Name = c.Course?.Name ?? "Unknown Course",
                AllLessons = totalLessons,
                WatchedLessons = c.LastLessonIdx,
                CompletionPercentage = totalLessons > 0
                    ? (decimal)c.LastLessonIdx / totalLessons * 100
                    : 0,
                ThumbnailUrl = c.Course?.ThumbnailUrl ?? string.Empty,
            };
        });

        return (totalCourses, result);
    }

    public async Task<bool> IsCourseOwner(int courseId, int userId)
    {
        return await dbContext.CourseProgresses
            .AnyAsync(cp => cp.CourseId == courseId && cp.SystemUserId == userId);
    }
}