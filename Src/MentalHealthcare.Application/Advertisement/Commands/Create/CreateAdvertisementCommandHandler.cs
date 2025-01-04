using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Commands.Create;

public class CreateAdvertisementCommandHandler(
    ILogger<CreateAdvertisementCommandHandler> logger,
    IAdvertisementRepository adRepository,
    IConfiguration configuration,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<CreateAdvertisementCommand, int>
{
    public async Task<int> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateAdvertisementCommand for Advertisement: {AdName}",
            request.AdvertisementName);

        // Authorize user
        logger.LogInformation("Authorizing user for creating advertisement.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to create advertisements.", currentUser.Id);

        // Validate image sizes
        ValidateImageSizes(request);

        // Map to entity
        var newAd = mapper.Map<Domain.Entities.Advertisement>(request);
        await adRepository.CreateAdvertisementAsync(newAd);

        var bunnyClient = new BunnyClient(configuration);
        logger.LogInformation("Starting image uploads for Advertisement ID: {AdId}", newAd.AdvertisementId);

        foreach (var img in request.Images)
        {
            var newImageName = $"{newAd.AdvertisementId}_{newAd.LastUploadImgCnt}.jpeg";
            newAd.LastUploadImgCnt++; // Increment image count for uniqueness

            // Upload image
            var response = await bunnyClient.UploadFileAsync(img, newImageName, Global.AdvertisementFolderName);

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning("Failed to upload image for Advertisement: {AdName}. Error: {Error}",
                    request.AdvertisementName, response.Message ?? "Unknown error");
                continue;
            }

            logger.LogInformation("Successfully uploaded image for Advertisement ID: {AdId}. URL: {Url}",
                newAd.AdvertisementId, response.Url);

            newAd.AdvertisementImageUrls.Add(new AdvertisementImageUrl
            {
                ImageUrl = response.Url,
                Advertisement = newAd
            });
        }

        // Update advertisement status
        if (!newAd.AdvertisementImageUrls.Any())
        {
            logger.LogWarning("No images were successfully uploaded for Advertisement ID: {AdId}. Marking as inactive.",
                newAd.AdvertisementId);
            newAd.IsActive = false;
        }

        await adRepository.UpdateAdvertisementAsync(newAd);

        logger.LogInformation("Advertisement ID: {AdId} created successfully with {ImageCount} images.",
            newAd.AdvertisementId, newAd.AdvertisementImageUrls.Count);
        return newAd.AdvertisementId;
    }

    /// <summary>
    /// Validates the size of the images in the request.
    /// </summary>
    /// <param name="request">The request containing the images to validate.</param>
    /// <exception cref="ArgumentException">Thrown if any image exceeds the allowed size limit.</exception>
    private void ValidateImageSizes(CreateAdvertisementCommand request)
    {
        foreach (var img in request.Images)
        {
            var imgSizeInMb = img.Length / (1 << 20); // Convert bytes to MB
            if (imgSizeInMb > Global.AdvertisementImgSize)
            {
                logger.LogWarning("Image size validation failed. Image size: {SizeInMb} MB, Allowed size: {MaxSize} MB",
                    imgSizeInMb, Global.AdvertisementImgSize);
                throw new ArgumentException($"Image size cannot be greater than {Global.AdvertisementImgSize} MB");
            }
        }
    }
}