using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseInteractionsRepository
{
    public Task Enroll(int courseId, int userId);
    public Task CompleteLessonAsync(int courseId, int lessonId, int userId);
    public Task<CourseLessonDto> GetLessonAsync(int lessonId);

    public Task<(int count, IEnumerable<CourseActivityDto> courses)> GetActiveCourseProgress(
        int userId,
        int pageNumber,
        int pageSize,
        string? courseName
    );
}