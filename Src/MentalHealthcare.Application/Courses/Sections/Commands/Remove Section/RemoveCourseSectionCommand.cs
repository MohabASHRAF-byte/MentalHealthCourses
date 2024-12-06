using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;

public class RemoveCourseSectionCommand : IRequest
{
    [JsonIgnore] public int CourseId { get; set; }
    [JsonIgnore] public int SectionId { get; set; }
}