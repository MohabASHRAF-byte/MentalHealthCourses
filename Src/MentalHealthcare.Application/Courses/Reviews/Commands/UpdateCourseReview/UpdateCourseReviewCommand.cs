using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.Reviews.Commands.UpdateCourseReview;

public class UpdateCourseReviewCommand : IRequest
{
    [JsonIgnore] public int CourseId { get; set; }
    [JsonIgnore] public int ReviewId { get; set; }
    public float? Rating { get; set; }
    public string? Content { get; set; } = string.Empty;
}