using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Profile.GetUserProfileData;

public class GetUserProfileDataQueryHandler(
    ILogger<GetUserProfileDataQueryHandler> logger,
    IUserRepository systemUserRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<GetUserProfileDataQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetUserProfileDataQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetUserProfileDataQuery for User.");

        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

        logger.LogInformation("Fetching profile data for UserId: {UserId}", currentUser.SysUserId);

        var userProfile = await systemUserRepository.GetUserProfileByIdAsync(
            currentUser.SysUserId!.Value
        );

        if (userProfile == null)
        {
            logger.LogWarning("No profile data found for UserId: {UserId}", currentUser.SysUserId);
            throw new BadHttpRequestException(
                localizationService.GetMessage("ProfileDataNotFound", "Profile data not found.")
            );
        }

        logger.LogInformation("Successfully fetched profile data for UserId: {UserId}", currentUser.SysUserId);

        return userProfile;
    }
}