using MentalHealthcare.Application.Courses;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface ICourseRepository
{
    public Task<int> CreateAsync(Course course);
    public Task UpdateCourse(Course course);
    public Task<Course> GetByIdAsync(int id);
    public Task<Course> GetCourseByIdAsync(int id);
    
    public Task<(int, IEnumerable<Course>)> GetAllAsync(string? search, int requestPageNumber, int requestPageSize);
    public Task SaveChangesAsync();
    public Task AddPendingUpload(PendingVideoUpload pendingVideoUpload);
    public Task DeletePending(string requestVideoId);
    public Task<PendingVideoUpload> GetPendingUpload(string requestVideoId);
    public int GetVideoOrder(int pendingCourseId);
    public Task AddCourseMatrial(CourseMateriel courseMateriel);
    public Task<int> AddCourseSection(CourseSection courseSection);
    
    public Task<List<CourseSection>> GetCourseSections(int courseId);
    public Task UpdateCourseSectionsAsync(List<CourseSection> courseSection);
    
    public Task DeleteCourseSectionAsync(CourseSection courseSection);
    public Task<CourseSection> GetCourseSectionByIdAsync(int id);
    
    public Task<int> AddCourseLesson(CourseLesson courseLesson);
    public Task<List<CourseLesson>> GetCourseLessons(int courseId,int sectionId);
    public Task DeleteCourseLessonAsync(CourseLesson targetLesson);
    public Task UpdateCourseLessonsAsync(List<CourseLesson> updatedLessons);
    public Task<CourseLesson> GetCourseLessonByIdAsync(int id);
    public Task<CourseMateriel> GetCourseMaterielByIdAsync(int id);
}