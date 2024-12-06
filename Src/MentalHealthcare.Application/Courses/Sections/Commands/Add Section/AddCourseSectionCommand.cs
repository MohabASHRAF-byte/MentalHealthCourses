using MediatR;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;

public class AddCourseSectionCommand:IRequest<int>
{
    public int CourseId { get; set; }
    public string Name { get; set; } = string.Empty;
}