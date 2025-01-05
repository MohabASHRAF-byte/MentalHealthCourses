using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseReview
{
    public Task AddCourseReviewAsync(UserReview review);

    public Task<(int count, IEnumerable<UserReviewDto> reviews)> GetCoursesReviewsAsync(
        int courseId,
        int pageNumber,
        int pageSize,
        int contentLimit
    );

    public Task UpdateCourseReviewAsync(
        int userId,
        int courseId,
        int reviewId,
        float? rating,
        string? content
    );

    public Task DeleteCourseReviewAsync(
        int? userId,
        int courseId,
        int reviewId
    );

    public Task<UserReviewDto> GetUserReviewAsync(
        int courseId,
        int reviewId
    );
}