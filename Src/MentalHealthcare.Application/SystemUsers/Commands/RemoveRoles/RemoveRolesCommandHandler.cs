using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers.Commands.AddRoles;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;

public class RemoveRolesCommandHandler(
    ILogger<AddRolesCommandHandler> logger,
    IUserRepository systemUserRepository,
    IUserContext userContext
) : IRequestHandler<RemoveRolesCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(RemoveRolesCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning("Unauthorized attempt to delete from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }

        var adminTenant = userContext.GetCurrentUser()?.Tenant;
        if (string.IsNullOrEmpty(adminTenant))
        {
            return OperationResult<string>.Failure("Unauthorized", StateCode.Unauthorized);
        }

        var changedUserRoles = await systemUserRepository.GetUserRolesAsync(request.UserName, adminTenant);
        if (changedUserRoles == -1)
        {
            return OperationResult<string>.Failure("User does not exist");
        }

        var roles = new List<UserRoles>() { UserRoles.Admin };

        foreach (var role in roles)
        {
            changedUserRoles &= ~(1 << (int)role);
        }

        await systemUserRepository.SetUserRolesAsync(request.UserName, adminTenant, changedUserRoles);

        logger.LogInformation("Successfully removed roles from user: {UserName}", request.UserName);
        return OperationResult<string>.SuccessResult(null, "Roles removed successfully");
    }
}
/*
 *
 *
 *   application
 *  domin
 */