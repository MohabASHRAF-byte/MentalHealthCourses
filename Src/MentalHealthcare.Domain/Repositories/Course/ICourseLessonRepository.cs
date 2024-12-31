using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseLessonRepository
{
    public Task<int> AddCourseLesson(CourseLesson courseLesson);
    public Task<bool> IsSectionOrderable(int courseId, int sectionId);
    public Task<List<CourseLesson>> GetCourseLessons(int courseId, int sectionId);
    public Task<List<CourseLessonViewDto>> GetCourseLessonsDto(int courseId, int sectionId);

    public Task DeleteCourseLessonAsync(CourseLesson targetLesson);
    public Task UpdateCourseLessonsAsync(List<CourseLesson> updatedLessons);
    public Task<CourseLesson> GetCourseLessonByIdAsync(int id);
    public Task<CourseLesson> GetCourseLessonByIdAsync(int courseId, int sectionId, int lessonId);
    public Task<CourseLesson> GetCourseFullLessonByIdAsync(int id);
    public Task UpdateCourseLessonDataAsync(int lessonId, string name);
    public Task RemoveLesson(int requestCourseId, int requestSectionId, int requestLessonId);
}