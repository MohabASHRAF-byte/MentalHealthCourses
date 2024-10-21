using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
            throw new InvalidOperationException("User Context is not present");
        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return null;
        var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userName = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
        var Roles = user.FindFirst(c => c.Type == Global.Roles)?.Value;
        var Tenant = user.FindFirst(c => c.Type == Global.TenantClaimType)?.Value;
        //todo handle nulls to avoid conflicts
        int.TryParse(Roles, out var roles);
        var currentUser = new CurrentUser(id!, userName!, roles, Tenant);
        return currentUser;
    }
}