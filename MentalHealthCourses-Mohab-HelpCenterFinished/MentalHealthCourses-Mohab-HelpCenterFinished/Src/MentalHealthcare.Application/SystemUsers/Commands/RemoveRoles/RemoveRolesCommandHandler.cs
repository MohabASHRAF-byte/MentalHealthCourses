using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers.Commands.AddRoles;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;

public class RemoveRolesCommandHandler(
    ILogger<AddRolesCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository,
    IUserContext userContext
) : IRequestHandler<RemoveRolesCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(RemoveRolesCommand request, CancellationToken cancellationToken)
    {
        // TODO: Check if the user has admin or manager privileges
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

        foreach (var role in request.Roles)
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