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
        userContext.EnsureAuthorizedUser([UserRoles.Admin],logger);

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