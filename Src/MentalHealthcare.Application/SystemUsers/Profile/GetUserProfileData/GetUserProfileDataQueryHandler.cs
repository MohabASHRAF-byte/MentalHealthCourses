using MediatR;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Profile.GetUserProfileData;

public class GetUserProfileDataQueryHandler(
    ILogger<GetUserProfileDataQueryHandler> logger,
    IUserRepository systemUserRepository,
    IUserContext userContext
) : IRequestHandler<GetUserProfileDataQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetUserProfileDataQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetUserProfileDataQuery for User.");

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to fetch profile data. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to access this profile.");
        }

        logger.LogInformation("Fetching profile data for UserId: {UserId}", currentUser.SysUserId);

        var userProfile = await systemUserRepository.GetUserProfileByIdAsync(
            currentUser.SysUserId!.Value
        );

        if (userProfile == null)
        {
            logger.LogWarning("No profile data found for UserId: {UserId}", currentUser.SysUserId);
            throw new ForBidenException("Profile data not found.");
        }

        logger.LogInformation("Successfully fetched profile data for UserId: {UserId}", currentUser.SysUserId);

        return userProfile;
    }
}