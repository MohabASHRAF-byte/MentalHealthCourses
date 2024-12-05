using MediatR;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Update;

public class UpdateHelpCenterCommand:IRequest
{
    public Domain.Entities.HelpCenterItem Item { get; set; }
}