using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories.Course;

public class CourseLessonRepository(
    MentalHealthDbContext dbContext,
    ILogger<CourseLessonRepository> logger
    ):ICourseLessonRepository
{
    public async Task<int> AddCourseLesson(CourseLesson courseLesson)
    {
        var maxOrder = await dbContext.CourseLessons
            .Where(c=>c.CourseSectionId == courseLesson.CourseSectionId
            )
            .MaxAsync(c => (int?)c.Order) ?? 0;

        courseLesson.Order = maxOrder + 1;
        dbContext.CourseLessons.Add(courseLesson);
        await dbContext.SaveChangesAsync();
        return courseLesson.CourseLessonId;
    }

    public async Task<List<CourseLesson>> GetCourseLessons(int courseId, int sectionId)
    {
        var lessons = dbContext.CourseLessons
            .Where(cs=>cs.CourseSectionId == sectionId
            );
        return await lessons.ToListAsync();
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
            .FirstOrDefaultAsync(c => c.CourseLessonId == id);
        if (courseLesson == null)
        {
            throw new ResourceNotFound(nameof(courseLesson), id.ToString());
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