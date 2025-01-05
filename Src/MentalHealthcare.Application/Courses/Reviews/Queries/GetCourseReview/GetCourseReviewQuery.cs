using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Reviews.Queries.GetCourseReview;

public class GetCourseReviewQuery : IRequest<UserReviewDto>
{
    public int CourseId { get; set; }
    public int ReviewId { get; set; }
}