using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Commands.Delete;

public class DeleteAdvertisementCommandHandler(
    ILogger<DeleteAdvertisementCommandHandler> logger,
    IAdvertisementRepository adRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<DeleteAdvertisementCommand>
{
    public async Task Handle(DeleteAdvertisementCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteAdvertisementCommand for Advertisement ID: {AdId}",
            request.AdvertisementId);

        // Authorize user
        logger.LogInformation("Authorizing user for deleting advertisement.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to delete advertisements.", currentUser.Id);

        // Fetch advertisement
        logger.LogInformation("Fetching advertisement with ID: {AdId}", request.AdvertisementId);
        var ad = await adRepository.GetAdvertisementByIdAsync(request.AdvertisementId);
        if (ad == null)
        {
            logger.LogWarning("Advertisement with ID: {AdId} not found.", request.AdvertisementId);
            throw new KeyNotFoundException($"Advertisement with ID {request.AdvertisementId} not found.");
        }

        var bunnyClient = new BunnyClient(configuration);

        // Delete images from BunnyCDN
        logger.LogInformation("Deleting images for Advertisement ID: {AdId}", request.AdvertisementId);
        foreach (var img in ad.AdvertisementImageUrls)
        {
            var imgName = GetImageName(img.ImageUrl);
            var response = await bunnyClient.DeleteFileAsync(imgName, Global.AdvertisementFolderName);

            if (!response.IsSuccessful)
            {
                logger.LogWarning("Failed to delete image: {ImgName} for Advertisement ID: {AdId}. Error: {Error}",
                    imgName, request.AdvertisementId, response.Message ?? "Unknown error");
            }
            else
            {
                logger.LogInformation("Successfully deleted image: {ImgName} for Advertisement ID: {AdId}", imgName,
                    request.AdvertisementId);
            }
        }

        // Delete advertisement from the repository
        logger.LogInformation("Deleting advertisement record for Advertisement ID: {AdId}", request.AdvertisementId);
        await adRepository.DeleteAdvertisementAsync(request.AdvertisementId);

        logger.LogInformation("Advertisement ID: {AdId} deleted successfully.", request.AdvertisementId);
    }

    /// <summary>
    /// Extracts the image name from a URL.
    /// </summary>
    /// <param name="url">The full URL of the image.</param>
    /// <returns>The image name.</returns>
    private string GetImageName(string url)
    {
        return url.Split('/').Last();
    }
}