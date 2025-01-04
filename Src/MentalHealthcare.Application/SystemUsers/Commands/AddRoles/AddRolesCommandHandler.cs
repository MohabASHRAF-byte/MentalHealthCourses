using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.AddRoles;

public class AddRolesCommandHandler(
    ILogger<AddRolesCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository,
    IUserContext userContext
) : IRequestHandler<AddRolesCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(AddRolesCommand request, CancellationToken cancellationToken)
    {
        userContext.EnsureAuthorizedUser(
            [UserRoles.Admin], logger);
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

        var roles = new List<UserRoles>(){ UserRoles.Admin };
        foreach (var Role in roles)
        {
            var val = (int)Role;
            changedUserRoles |= (uint)(1 << val);
        }

        await systemUserRepository.SetUserRolesAsync(request.UserName, adminTenant, changedUserRoles);
        return OperationResult<string>.SuccessResult(null, "Success");
    }
}

/* presentiation
 * infrastrucue ==
 * application
 * domain
 */