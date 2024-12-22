using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseRepository
{
    public Task<int> CreateAsync(Entities.Course course);
    public Task UpdateCourse(Entities.Course course);
    public Task<Entities.Course> GetFullCourseByIdAsync(int id);
    public Task<Entities.Course> GetMinimalCourseByIdAsync(int id);
    
    public Task<(int, IEnumerable<Entities.Course>)> GetAllAsync(string? search, int requestPageNumber, int requestPageSize);
    public Task SaveChangesAsync();
    public Task AddPendingUpload(PendingVideoUpload pendingVideoUpload);
    public Task DeletePending(string requestVideoId);
    public Task<PendingVideoUpload> GetPendingUpload(string requestVideoId);
    public Task<string> GetCourseCollectionId(int courseId);
    
    public Task<string> GetCourseName(int courseId);
    public Task<bool> DoesCourseExist(int courseId);
    
}