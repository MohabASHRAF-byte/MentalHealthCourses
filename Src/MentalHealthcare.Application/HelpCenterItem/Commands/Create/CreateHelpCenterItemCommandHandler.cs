using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Create;

public class CreateHelpCenterItemCommandHandler(
    ILogger<CreateHelpCenterItemCommandHandler> logger,
    IHelpCenterRepository helpCenterRepository,
    IUserContext userContext
) : IRequestHandler<CreateHelpCenterItemCommand, int>
{
    public async Task<int> Handle(CreateHelpCenterItemCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateHelpCenterItemCommand for item: {Name}", request.Name);

        // Authorize user
        logger.LogInformation("Authorizing user for creating HelpCenter item.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to create HelpCenter items.", currentUser.Id);

        // Validate HelpCenterItemType
        logger.LogInformation("Validating HelpCenterItemType: {Type}", request.HelpCenterItemType);
        if (!Enum.IsDefined(typeof(Global.HelpCenterItems), request.HelpCenterItemType))
        {
            logger.LogWarning("Invalid HelpCenterItemType: {Type}", request.HelpCenterItemType);
            throw new ArgumentException(
                $"Invalid value for {nameof(request.HelpCenterItemType)}: {request.HelpCenterItemType}");
        }

        // Create new HelpCenterItem entity
        var term = new Domain.Entities.HelpCenterItem
        {
            Type = request.HelpCenterItemType,
            Name = request.Name,
            Description = request.Description,
        };

        // Persist to repository
        logger.LogInformation("Adding new HelpCenter item to the repository.");
        var newTermId = await helpCenterRepository.AddAsync(term);
        logger.LogInformation("Successfully added HelpCenter item with ID: {Id} and Name: {Name}", newTermId,
            term.Name);

        return newTermId;
    }
}