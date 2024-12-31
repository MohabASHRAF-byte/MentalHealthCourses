using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.Course.Commands.UpdateCourseCommand;

public class UpdateCourseCommand : IRequest
{
    [JsonIgnore] public int CourseId { get; set; }
    public string? Name { set; get; } = string.Empty;
    public decimal? Price { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int? InstructorId { get; set; } = default!;
    public List<int>? CategoryId { get; set; } = [];
    public bool? IsFree { get; set; } = false;
    public bool? IsFeatured { get; set; } = false;
    public bool? IsArchived { get; set; } = false;
}