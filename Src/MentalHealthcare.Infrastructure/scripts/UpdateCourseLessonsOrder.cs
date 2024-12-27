using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.scripts;

public static class UpdateCourseLessonsOrderExtend
{
    public static async Task UpdateCourseLessonsOrder(this MentalHealthDbContext dbContext, int? courseId = null,
        int? lessonId = null)
    {
        // If both courseId and lessonId are null, return
        if (courseId == null && lessonId == null)
        {
            return;
        }

        // If courseId is null, determine it using lessonId
        if (courseId == null && lessonId != null)
        {
            courseId = await dbContext.CourseLessons
                .Where(l => l.CourseLessonId == lessonId)
                .Select(l => l.CourseSection.CourseId)
                .FirstOrDefaultAsync();

            if (courseId == 0)
            {
                return; // If lessonId is invalid, return
            }
        }

        // Fetch the course with its sections and lessons in order
        var course = await dbContext.Courses
            .Where(c => c.CourseId == courseId)
            .Include(c => c.CourseSections.OrderBy(cs => cs.Order)) // Order sections by their order property
            .ThenInclude(cs => cs.Lessons.OrderBy(cl => cl.Order)) // Order lessons within sections
            .FirstOrDefaultAsync();

        if (course == null)
        {
            return;
        }


        // Sort sections and lessons after retrieval
        course.CourseSections = course.CourseSections
            .OrderBy(cs => cs.Order)
            .ToList();

        foreach (var section in course.CourseSections)
        {
            section.Lessons = section.Lessons
                .OrderBy(lesson => lesson.Order)
                .ToList();
        }


        // Update lesson order within the course
        var lessonOrder = 1;
        foreach (var section in course.CourseSections)
        {
            foreach (var lesson in section.Lessons)
            {
                lesson.OrderOnCourse = lessonOrder;
                lessonOrder++;
            }
        }
    }
}