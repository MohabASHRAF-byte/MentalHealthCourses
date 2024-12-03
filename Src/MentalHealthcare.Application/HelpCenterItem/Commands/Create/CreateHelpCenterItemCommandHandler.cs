using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.HelpCenterItem.Commands.Create;

public class CreateHelpCenterItemCommandHandler(
    ILogger<CreateHelpCenterItemCommandHandler> logger,
    IHelpCenterRepository helpCenterRepository,
    IUserContext userContext
    ):IRequestHandler<CreateHelpCenterItemCommand, int>
{
    public async Task<int> Handle(CreateHelpCenterItemCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.IsAuthorized([UserRoles.Admin]))
        {
            logger.LogWarning("Unauthorized attempt to add HelpCenter Item by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission add HelpCenter Item");
        }
        logger.LogInformation("CreateHelpCenterItemCommandHandler for @{n}",request.Name);
        if (!Enum.IsDefined(typeof(Global.HelpCenterItems), request.HelpCenterItemType))
        {
            logger.LogWarning("Invalid HelpCenterItemType: @{type}", request.HelpCenterItemType);
            throw new ArgumentException($"Invalid value for {nameof(request.HelpCenterItemType)}: {request.HelpCenterItemType}");
        }
        var term = new Domain.Entities.HelpCenterItem
        {
            Type = request.HelpCenterItemType,
            Name = request.Name,
            Description = request.Description,
        };
        var newTermId = await helpCenterRepository.AddAsync(term);
        logger.LogInformation("CreateHelpCenterItemCommandHandler for @{n} was added with name @{}",newTermId,term.Name);
        return newTermId;
    }
}