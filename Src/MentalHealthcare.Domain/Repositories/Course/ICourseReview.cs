using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories.Course;

public interface ICourseReview
{
    public Task AddCourseReviewAsync(UserReview review);
}