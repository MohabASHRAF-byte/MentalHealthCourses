using MediatR;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_order;

public class UpdateSectionsOrderCommand : IRequest
{
    public int CourseId { get; set; }
    public List<SectionOrderDto> Orders { get; set; } = new();
}

public class SectionOrderDto
{
    public int SectionId { get; set; }
    public int Order { get; set; }
}