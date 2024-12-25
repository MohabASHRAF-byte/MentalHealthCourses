using System.Security.Claims;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.SystemUsers;

public interface IUserContext
{
    public CurrentUser? GetCurrentUser();
}

internal class UserContext(
    IHttpContextAccessor httpContextAccessor
) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            throw new InvalidOperationException("User Context is not present");
        }

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = user.FindFirstValue(ClaimTypes.Name);
        var rolesClaim = user.FindFirstValue(Global.Roles);
        var tenant = user.FindFirstValue(Global.TenantClaimType);
        var systemUserClaim = user.FindFirstValue(Global.UserIdClaimType);
        var adminClaim = user.FindFirstValue(Global.AdminIdClaimType);

        int? systemUserId = null;
        int? adminId = null;

        if (int.TryParse(systemUserClaim, out var parsedSystemUserId))
        {
            systemUserId = parsedSystemUserId;
        }

        if (int.TryParse(adminClaim, out var parsedAdminId))
        {
            adminId = parsedAdminId;
        }

        if (!int.TryParse(rolesClaim, out var roles))
        {
            roles = 0; 
        }

        return new CurrentUser(
            id!,
            userName!,
            roles,
            tenant ?? "",
            systemUserId,
            adminId
        );
    }
}