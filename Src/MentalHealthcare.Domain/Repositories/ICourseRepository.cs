using MentalHealthcare.Application.Courses;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface ICourseRepository
{
    public Task<int> CreateAsync(Course course);
    public Task<CourseDto> GetByIdAsync(int id);
    public Task<Course> GetCourseByIdAsync(int id);
    
    public Task<(int, IEnumerable<Course>)> GetAllAsync(string? search, int requestPageNumber, int requestPageSize);
    public Task SaveChangesAsync();
    public Task AddPendingUpload(PendingVideoUpload pendingVideoUpload);
    public Task DeletePending(string requestVideoId);
    public Task<PendingVideoUpload> GetPendingUpload(string requestVideoId);
    public int GetVideoOrder(int pendingCourseId);
    public Task AddCourseMatrial(CourseMateriel courseMateriel);

}