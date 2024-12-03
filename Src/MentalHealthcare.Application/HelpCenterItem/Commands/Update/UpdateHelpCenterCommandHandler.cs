using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Update;

public class UpdateHelpCenterCommandHandler(
    ILogger<UpdateHelpCenterCommandHandler> logger,
    IHelpCenterRepository helpCenterRepository,
    IUserContext userContext
    ):IRequestHandler<UpdateHelpCenterCommand>
{
    public async Task Handle(UpdateHelpCenterCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.IsAuthorized([UserRoles.Admin]))
        {
            logger.LogWarning("Unauthorized attempt to update HelpCenter Item by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission update HelpCenter Item");
        }
        logger.LogInformation(@"Updated term {}",request.Item.Name);
        await helpCenterRepository.Update(request.Item);
        
    }
}