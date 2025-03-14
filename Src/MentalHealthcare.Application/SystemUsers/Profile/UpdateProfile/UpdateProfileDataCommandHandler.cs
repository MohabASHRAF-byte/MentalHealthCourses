using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;

public class UpdateProfileDataCommandHandler(
    ILogger<UpdateProfileDataCommandHandler> logger,
    IUserRepository systemUserRepository,
    IUserContext userContext ,
    IConfiguration configuration
) : IRequestHandler<UpdateProfileDataCommand>
{
    public async Task Handle(UpdateProfileDataCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateProfileDataCommand for User: {RequestDetails}",
            new { request.FirstName, request.LastName, request.PhoneNumber, request.BirthDate, request.ProfilePic });
                                   //TODO: Change in UserRoles From Admin => User
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

        logger.LogInformation("Updating profile for UserId: {UserId} with new details: {RequestDetails}",
            currentUser.SysUserId,
            new { request.FirstName, request.LastName, request.PhoneNumber, request.BirthDate , request.ProfilePic });


        string? profilePicUrl = null;


        if (request.ProfilePic != null)
        {
            logger.LogInformation("Uploading profile picture for User: {UserId}", currentUser.SysUserId);

            var bunnyClient = new BunnyClient(configuration);

            // Generate unique file name
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ProfilePic.FileName)}";

            // Upload file
            var uploadResult = await bunnyClient.UploadFileAsync(request.ProfilePic, fileName, "Users");

            if (!uploadResult.IsSuccessful)
            {
                logger.LogWarning("Failed to upload profile picture for User: {UserId}, Error: {Error}",
                    currentUser.SysUserId, uploadResult.Message);
            }

            // Store uploaded URL
            profilePicUrl = uploadResult.Url;

            // (Optional) Clear Bunny CDN cache if needed
            await bunnyClient.ClearCacheAsync(uploadResult.Url!);






        }



        await systemUserRepository.UpdateUserProfileAsync(
            currentUser.SysUserId!.Value,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.BirthDate ,
            profilePicUrl

        );

        logger.LogInformation("Successfully updated profile for UserId: {UserId}", currentUser.SysUserId);
    }
}