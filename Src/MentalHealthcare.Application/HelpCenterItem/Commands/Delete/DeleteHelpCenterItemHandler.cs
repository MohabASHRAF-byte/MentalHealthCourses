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
        logger.LogInformation("Handling DeleteHelpCenterItemCommand for item ID: {HelpCenterId}", request.HelpCenterId);

        // Authorize user
        logger.LogInformation("Authorizing user for deleting HelpCenter item.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin ], logger);
        logger.LogInformation("User {UserId} authorized to delete HelpCenter items.", currentUser.Id);

        // Delete HelpCenter item
        logger.LogInformation("Deleting HelpCenter item with ID: {HelpCenterId}.", request.HelpCenterId);
        await helpCenterRepository.DeleteAsync(request.HelpCenterId);
        logger.LogInformation("HelpCenter item with ID: {HelpCenterId} deleted successfully.", request.HelpCenterId);
    }
}