using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseRepository
{
    public Task<int> CreateAsync(Entities.Course course ,List<int> categories);
    public Task UpdateCourse(Entities.Course course);
    public Task<Entities.Course> GetFullCourseByIdAsync(
        int id
        );
    public Task<Entities.Course> GetMinimalCourseByIdAsync(int id);

    public Task<(int TotalCount, IEnumerable<CourseViewDto> Courses)> GetAllAsync(
        int? userId,
        string? searchName,
        int requestPageNumber,
        int requestPageSize);

    public Task SaveChangesAsync();
    public Task AddPendingUpload(PendingVideoUpload pendingVideoUpload);
    public Task DeletePending(string requestVideoId);
    public Task<PendingVideoUpload> GetPendingUpload(string requestVideoId);
    public Task<string> GetCourseCollectionId(int courseId);

    public Task<string> GetCourseName(int courseId);
    public Task<bool> DoesCourseExist(int courseId);
}