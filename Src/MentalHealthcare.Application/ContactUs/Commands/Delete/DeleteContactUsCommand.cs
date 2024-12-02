using MediatR;

namespace MentalHealthcare.Application.ContactUs.Commands.Delete;

public class DeleteContactUsCommand:IRequest
{
    public List<int> FormsId { get; set; } = [];
}