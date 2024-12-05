using MediatR;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Delete;

public class DeleteHelpCenterItemCommand:IRequest
{
    public int HelpCenterId { get; set; }
}