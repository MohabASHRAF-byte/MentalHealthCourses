using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseRepository
{
    public Task<int> CreateAsync(Entities.Courses.Course course, List<int> categories);

    public Task<int> UpdateCourseAsync(
        int courseId,
        string? name,
        decimal? price,
        string? description,
        int? instructorId,
        List<int>? categoryId,
        bool? isFree,
        bool? isFeatured,
        bool? isArchived);

    public Task UpdateCourse(Entities.Courses.Course course);

    public Task<Entities.Courses.Course> GetFullCourseByIdAsync(
        int id
    );

    public Task<Entities.Courses.Course> GetMinimalCourseByIdAsync(int id);

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

    public Task<bool> UpdateCourseProgressAfterDeletingSection(
        int courseId,
        int sectionId,
        int sectionStart,
        int sectionEnd
    );

    public Task<bool> IsEnrolledInCourse(int courseId, int userId);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}