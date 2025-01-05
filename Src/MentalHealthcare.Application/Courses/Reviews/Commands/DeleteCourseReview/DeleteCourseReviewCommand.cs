using MediatR;

namespace MentalHealthcare.Application.Courses.Reviews.Commands.DeleteCourseReview;

public class DeleteCourseReviewCommand : IRequest
{
    public int CourseId { get; set; }
    public int ReviewId { get; set; }
}