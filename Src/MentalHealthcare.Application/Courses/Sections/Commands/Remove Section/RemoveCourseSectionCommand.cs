using MediatR;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;

public class RemoveCourseSectionCommand : IRequest
{
    public int CourseId { get; set; }
    public int SectionId { get; set; }
}