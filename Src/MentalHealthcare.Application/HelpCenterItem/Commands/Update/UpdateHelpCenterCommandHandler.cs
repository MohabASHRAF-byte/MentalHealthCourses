using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Update;

public class UpdateHelpCenterCommandHandler(
    ILogger<UpdateHelpCenterCommandHandler> logger,
    IHelpCenterRepository helpCenterRepository
    ):IRequestHandler<UpdateHelpCenterCommand>
{
    public async Task Handle(UpdateHelpCenterCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(@"Updated term {}",request.Item.Name);
        await helpCenterRepository.Update(request.Item);
        
    }
}