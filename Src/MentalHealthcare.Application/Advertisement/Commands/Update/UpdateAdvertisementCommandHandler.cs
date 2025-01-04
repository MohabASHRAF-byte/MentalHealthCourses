using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Application.Advertisement.Commands.Update;

public class UpdateAdvertisementCommandHandler(
    ILogger<UpdateAdvertisementCommandHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<UpdateAdvertisementCommand, int>
{
    public async Task<int> Handle(UpdateAdvertisementCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting update process for Advertisement ID {AdId}.", request.AdvertisementId);

        // Authorize user
        logger.LogInformation("Authorizing user for updating advertisement.");
        var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
        logger.LogInformation("User {UserId} authorized to update advertisements.", currentUser.Id);

        // Validate Advertisement ID
        if (request.AdvertisementId == null)
        {
            logger.LogWarning("Advertisement ID is null. Aborting update process.");
            return 0;
        }

        request.Images ??= new();
        ValidateImageSizes(request);

        // Retrieve advertisement
        logger.LogInformation("Fetching advertisement with ID: {AdId}.", request.AdvertisementId);
        var advertisement = await advertisementRepository.GetAdvertisementByIdAsync((int)request.AdvertisementId);

        if (advertisement == null)
        {
            logger.LogError("Advertisement with ID {AdId} not found.", request.AdvertisementId);
            throw new KeyNotFoundException($"Advertisement with ID {request.AdvertisementId} not found.");
        }

        // Update advertisement details
        logger.LogInformation("Updating details for Advertisement ID: {AdId}.", request.AdvertisementId);
        UpdateAdvertisementDetails(ref advertisement, request);

        var bunnyClient = new BunnyClient(configuration);

        // Handle existing images
        logger.LogInformation("Handling existing images for Advertisement ID: {AdId}.", request.AdvertisementId);
        HandleExistingImages(ref advertisement, request, bunnyClient);

        // Upload new images
        logger.LogInformation("Uploading new images for Advertisement ID: {AdId}.", request.AdvertisementId);
        UploadNewImages(ref advertisement, request, bunnyClient);

        // Check for at least one image
        if (!advertisement.AdvertisementImageUrls.Any())
        {
            logger.LogWarning("No images found for Advertisement ID: {AdId}. Marking as inactive.", request.AdvertisementId);
            advertisement.IsActive = false;
        }

        // Persist changes
        logger.LogInformation("Saving changes for Advertisement ID: {AdId}.", request.AdvertisementId);
        await advertisementRepository.UpdateAdvertisementAsync(advertisement);

        logger.LogInformation("Advertisement ID: {AdId} updated successfully.", request.AdvertisementId);
        return advertisement.AdvertisementId;
    }

    private void ValidateImageSizes(UpdateAdvertisementCommand request)
    {
        foreach (var image in request.Images)
        {
            var imageSizeInMb = image.Length / (1 << 20);
            if (imageSizeInMb > Global.AdvertisementImgSize)
            {
                logger.LogWarning("Attempted to upload an image exceeding the allowed size: {ImageSize} MB.", imageSizeInMb);
                throw new Exception($"Image size cannot exceed {Global.AdvertisementImgSize} MB.");
            }
        }
    }

    private void UpdateAdvertisementDetails(ref Domain.Entities.Advertisement advertisement, UpdateAdvertisementCommand request)
    {
        if (request.IsActive.HasValue)
        {
            advertisement.IsActive = request.IsActive.Value;
            logger.LogInformation("Updated IsActive for Advertisement ID: {AdId} to {IsActive}.", advertisement.AdvertisementId, request.IsActive.Value);
        }

        if (!request.AdvertisementName.IsNullOrEmpty())
        {
            advertisement.AdvertisementName = request.AdvertisementName!;
            logger.LogInformation("Updated AdvertisementName for Advertisement ID: {AdId} to {AdName}.", advertisement.AdvertisementId, request.AdvertisementName);
        }

        if (!request.AdvertisementDescription.IsNullOrEmpty())
        {
            advertisement.AdvertisementDescription = request.AdvertisementDescription!;
            logger.LogInformation("Updated AdvertisementDescription for Advertisement ID: {AdId}.", advertisement.AdvertisementId);
        }
    }

    private void HandleExistingImages(
        ref Domain.Entities.Advertisement advertisement,
        UpdateAdvertisementCommand request,
        BunnyClient bunnyClient)
    {
        if (request.ImagesUrls != null && request.ImagesUrls.Count > 0)
        {
            var retainedImages = new List<AdvertisementImageUrl>();
            foreach (var image in advertisement.AdvertisementImageUrls.ToList())
            {
                var imageName = GetImageName(image.ImageUrl);
                if (request.ImagesUrls.Contains(image.ImageUrl))
                {
                    retainedImages.Add(image);
                    continue;
                }

                logger.LogInformation("Deleting image: {ImgName} for Advertisement ID: {AdId}.", imageName, advertisement.AdvertisementId);
                bunnyClient.DeleteFileAsync(imageName, Global.AdvertisementFolderName).Wait();
            }

            advertisementRepository.DeleteAdvertisementPhotosUrlsAsync(advertisement.AdvertisementId).Wait();
            advertisement.AdvertisementImageUrls = retainedImages;
        }
        else
        {
            logger.LogWarning("No image URLs provided for Advertisement ID: {AdId}. Removing all existing images.", advertisement.AdvertisementId);
            advertisement.AdvertisementImageUrls = new List<AdvertisementImageUrl>();
        }
    }

    private void UploadNewImages(
        ref Domain.Entities.Advertisement advertisement,
        UpdateAdvertisementCommand request,
        BunnyClient bunnyClient)
    {
        foreach (var image in request.Images!)
        {
            var newImageName = $"{advertisement.AdvertisementId}_{advertisement.LastUploadImgCnt}.jpeg";
            advertisement.LastUploadImgCnt++;
            var response = bunnyClient.UploadFileAsync(image, newImageName, Global.AdvertisementFolderName).Result;

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning("Failed to upload image for Advertisement ID: {AdId}. Error: {Message}", advertisement.AdvertisementId, response.Message ?? "Unknown error");
                continue;
            }

            logger.LogInformation("Successfully uploaded image for Advertisement ID: {AdId}. URL: {Url}", advertisement.AdvertisementId, response.Url);
            advertisement.AdvertisementImageUrls.Add(new AdvertisementImageUrl
            {
                ImageUrl = response.Url,
                Advertisement = advertisement
            });
        }
    }

    private string GetImageName(string url) => url.Split('/').Last();
}
