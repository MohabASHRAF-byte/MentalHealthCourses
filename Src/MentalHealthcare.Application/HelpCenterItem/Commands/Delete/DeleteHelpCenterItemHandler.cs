using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Delete;

public class DeleteHelpCenterItemHandler(
    ILogger<DeleteHelpCenterItemHandler> logger,
    IHelpCenterRepository helpCenterRepository
) : IRequestHandler<DeleteHelpCenterItemCommand>
{
    public async Task Handle(DeleteHelpCenterItemCommand request, CancellationToken cancellationToken)
    {
        //todo
        //add auth
        logger.LogInformation(@"Delete Term with id {}", request.HelpCenterId);
        await helpCenterRepository.DeleteAsync(request.HelpCenterId);
        logger.LogInformation(@"Term with id {} deleted", request.HelpCenterId);
    }
}