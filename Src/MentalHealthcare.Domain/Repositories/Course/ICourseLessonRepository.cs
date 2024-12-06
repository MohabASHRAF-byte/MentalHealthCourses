using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseLessonRepository
{
    public Task<int> AddCourseLesson(CourseLesson courseLesson);
    
    public Task<List<CourseLesson>> GetCourseLessons(int courseId,int sectionId);
    public Task<List<CourseLessonViewDto>> GetCourseLessonsDto(int courseId, int sectionId);

    public Task DeleteCourseLessonAsync(CourseLesson targetLesson);
    public Task UpdateCourseLessonsAsync(List<CourseLesson> updatedLessons);
    public Task<CourseLesson> GetCourseLessonByIdAsync(int id);
    public Task<CourseLesson> GetCourseFullLessonByIdAsync(int id);
    public Task UpdateCourseLessonDataAsync(int lessonId , string name);
}