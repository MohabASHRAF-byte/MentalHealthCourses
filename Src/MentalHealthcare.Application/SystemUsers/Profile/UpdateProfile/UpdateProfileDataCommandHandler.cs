using MediatR;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;

public class UpdateProfileDataCommandHandler(
    ILogger<UpdateProfileDataCommandHandler> logger,
    IUserRepository systemUserRepository,
    IUserContext userContext
) : IRequestHandler<UpdateProfileDataCommand>
{
    public async Task Handle(UpdateProfileDataCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateProfileDataCommand for User: {RequestDetails}",
            new { request.FirstName, request.LastName, request.PhoneNumber, request.BirthDate });

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to update profile. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to update this profile.");
        }

        logger.LogInformation("Updating profile for UserId: {UserId} with new details: {RequestDetails}",
            currentUser.SysUserId,
            new { request.FirstName, request.LastName, request.PhoneNumber, request.BirthDate });

        await systemUserRepository.UpdateUserProfileAsync(
            currentUser.SysUserId!.Value,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.BirthDate
        );

        logger.LogInformation("Successfully updated profile for UserId: {UserId}", currentUser.SysUserId);
    }
}