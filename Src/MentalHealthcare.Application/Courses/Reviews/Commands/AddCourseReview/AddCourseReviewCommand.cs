using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.Reviews.Commands.AddCourseReview;

public class AddCourseReviewCommand : IRequest<int>
{
    [JsonIgnore] public int CourseId { get; set; }
    public float Rating { get; set; }
    public string Content { get; set; } = string.Empty;
}