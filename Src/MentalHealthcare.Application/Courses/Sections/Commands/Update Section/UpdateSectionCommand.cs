using MediatR;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_Section;

public class UpdateSectionCommand : IRequest<int>
{
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    public string SectionName { get; set; } = string.Empty;
}