using System.Security.Claims;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers;

public interface IUserContext
{
    public CurrentUser? GetCurrentUser();
    public CurrentUser EnsureAuthorizedUser(List<UserRoles> requiredRoles, ILogger logger);
    public CurrentUser UserHaveAny(List<UserRoles> requiredRoles, ILogger logger);

}

internal class UserContext(
    IHttpContextAccessor httpContextAccessor,
    ILocalizationService localizationService
) : IUserContext
{

    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            throw new BadHttpRequestException(
                localizationService.GetMessage("UserNotFound")
            );
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

    public CurrentUser EnsureAuthorizedUser(List<UserRoles> requiredRoles, ILogger logger)
    {
        var currentUser = GetCurrentUser();

        if (currentUser == null || !currentUser.HasRole(requiredRoles))
        {
            var userId = currentUser?.Id ?? "Anonymous";
            logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
            throw new ForBidenException(
                localizationService.GetMessage("PermissionDenied")
            );
        }

        logger.LogInformation("User {UserId} authorized with roles: {Roles}", currentUser.Id,
            string.Join(",", requiredRoles));

        return currentUser;
    }
    public CurrentUser UserHaveAny(List<UserRoles> requiredRoles, ILogger logger)
    {
        var currentUser = GetCurrentUser();

        if (currentUser == null || !currentUser.IsAuthorized(requiredRoles))
        {
            var userId = currentUser?.Id ?? "Anonymous";
            logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
            throw new ForBidenException(
                localizationService.GetMessage("PermissionDenied")
            );
        }

        logger.LogInformation("User {UserId} authorized with roles: {Roles}", currentUser.Id,
            string.Join(",", requiredRoles));

        return currentUser;
    }
}