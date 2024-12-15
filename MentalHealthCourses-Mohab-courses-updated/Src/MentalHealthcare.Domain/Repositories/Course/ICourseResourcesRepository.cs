using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseResourcesRepository
{
    public Task<CourseLessonResource> AddCourseLessonResourceAsync(CourseLessonResource resource);
    public Task<CourseLessonResource> GetCourseLessonResourceByIdAsync(int id);
    public Task<List<CourseLessonResource>> GetCourseLessonResourcesByCourseIdAsync(int lessonId);
    public Task DeleteCourseLessonResourceAsync(CourseLessonResource resource);
    public Task SaveChangesAsync();
    public Task UpdateCourseLessonResourcesAsync(List<CourseLessonResource> resources);
}