using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Reviews.Queries.GetAllCourseReviews;

public class GetAllCourseReviewsQuery : IRequest<PageResult<UserReviewDto>>
{
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int ContentLimit { get; set; } = 100;
}