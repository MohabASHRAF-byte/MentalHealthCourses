using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Delete;

public class DeleteHelpCenterItemHandler(
    ILogger<DeleteHelpCenterItemHandler> logger,
    IHelpCenterRepository helpCenterRepository,
    IUserContext userContext
) : IRequestHandler<DeleteHelpCenterItemCommand>
{
    public async Task Handle(DeleteHelpCenterItemCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.IsAuthorized([UserRoles.Admin]))
        {
            logger.LogWarning("Unauthorized attempt to delete HelpCenter Item by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission delete HelpCenter Item");
        }
        logger.LogInformation(@"Delete Term with id {}", request.HelpCenterId);
        await helpCenterRepository.DeleteAsync(request.HelpCenterId);
        logger.LogInformation(@"Term with id {} deleted", request.HelpCenterId);
    }
}