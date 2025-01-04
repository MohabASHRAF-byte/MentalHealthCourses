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
) : IRequestHandler<UpdateHelpCenterCommand>
{
    public async Task Handle(UpdateHelpCenterCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateHelpCenterCommand for item: {ItemName}", request.Item.Name);

        // Authorize user
        logger.LogInformation("Authorizing user for updating HelpCenter item.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to update HelpCenter items.", currentUser.Id);

        // Update HelpCenter item
        logger.LogInformation("Updating HelpCenter item with ID: {ItemId} and Name: {ItemName}.", request.Item.HelpCenterItemId,
            request.Item.Name);
        await helpCenterRepository.Update(request.Item);
        logger.LogInformation("HelpCenter item with ID: {ItemId} updated successfully.", request.Item.HelpCenterItemId);
    }
}