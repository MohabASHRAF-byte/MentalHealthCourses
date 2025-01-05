using MediatR;
using MentalHealthcare.Domain.Dtos.course;

namespace MentalHealthcare.Application.Courses.Sections.Commands.Update_order;

public class UpdateSectionsOrderCommand : IRequest
{
    public int CourseId { get; set; }
    public List<SectionOrderDto> Orders { get; set; } = new();
}

