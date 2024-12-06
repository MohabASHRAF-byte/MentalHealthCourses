using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Application.Advertisement.Commands.Update;

/// <summary>
/// Handles updating an advertisement with new or existing data.
/// </summary>
/// <param name="request">The command containing details for updating the advertisement.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>A task representing the operation, containing the ID of the updated advertisement.</returns>
/// 
/// <remarks>
/// Logic flow:
/// <list type="number">
/// <item>
/// <description>Log the start of the advertisement update process.</description>
/// </item>
/// <item>
/// <description>Validate the input:
/// <list type="bullet">
/// <item>Check if <c>AdvertisementId</c> is null. Return 0 if invalid.</item>
/// <item>Ensure uploaded images do not exceed the allowed size.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Retrieve and update the advertisement's basic details:
/// <list type="bullet">
/// <item>Update fields such as <c>IsActive</c>, <c>AdvertisementName</c>, and <c>AdvertisementDescription</c>.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Manage images:
/// <list type="bullet">
/// <item>If `ImagesUrls` is empty, keep all current images.</item>
/// <item>If `ImagesUrls` has specific URLs, keep only those and delete others.</item>
/// <item>Upload new images and associate them with the advertisement.</item>
/// <item>Ensure at least one image is associated with the advertisement.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Persist the updated advertisement.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="Exception">Thrown if any validation fails or there is an issue updating the advertisement.</exception>
public class UpdateAdvertisementCommandHandler(
    ILogger<UpdateAdvertisementCommandHandler> logger,
    IAdvertisementRepository advertisementRepository,
    IMapper mapper,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<UpdateAdvertisementCommand, int>
{
    public async Task<int> Handle(UpdateAdvertisementCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Starting update process for Advertisement ID {request.AdvertisementId}.");
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning("Unauthorized attempt to update advertisement by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission to update advertisement.");
        }
        if (request.AdvertisementId == null)
            return 0;

        request.Images ??= new();
        ValidateImageSizes(request);

        var advertisement = await advertisementRepository.GetAdvertisementByIdAsync((int)request.AdvertisementId);
        UpdateAdvertisementDetails(ref advertisement, request);

        var bunnyClient = new BunnyClient(configuration);

        HandleExistingImages(ref advertisement, request, bunnyClient);
        UploadNewImages(ref advertisement, request, bunnyClient);
        if (!advertisement.AdvertisementImageUrls.Any())
            advertisement.IsActive = false;
        await advertisementRepository.UpdateAdvertisementAsync(advertisement);
        return advertisement.AdvertisementId;
    }

    private void ValidateImageSizes(UpdateAdvertisementCommand request)
    {
        foreach (var image in request.Images)
        {
            var imageSizeInMb = image.Length / (1 << 20);
            if (imageSizeInMb > Global.AdvertisementImgSize)
            {
                logger.LogWarning($"Attempted to upload an image exceeding the allowed size: {imageSizeInMb} MB.");
                throw new Exception($"Image size cannot exceed {Global.AdvertisementImgSize} MB.");
            }
        }
    }

    private void UpdateAdvertisementDetails(ref Domain.Entities.Advertisement advertisement, UpdateAdvertisementCommand request)
    {
        if (request.IsActive.HasValue)
            advertisement.IsActive = request.IsActive.Value;

        if (!request.AdvertisementName.IsNullOrEmpty())
            advertisement.AdvertisementName = request.AdvertisementName!;

        if (!request.AdvertisementDescription.IsNullOrEmpty())
            advertisement.AdvertisementDescription = request.AdvertisementDescription!;
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

                bunnyClient.DeleteFileAsync(imageName, Global.AdvertisementFolderName).Wait();
            }

            advertisementRepository.DeleteAdvertisementPhotosUrlsAsync(advertisement.AdvertisementId).Wait();
            advertisement.AdvertisementImageUrls = retainedImages;
        }else
        {
            advertisement.AdvertisementImageUrls = [];
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
                logger.LogWarning(
                    "Failed to upload image for Advertisement {AdvertisementName}. Error: {Message}",
                    request.AdvertisementName,
                    response.Message ?? ""
                );
                continue;
            }

            advertisement.AdvertisementImageUrls.Add(new AdvertisementImageUrl
            {
                ImageUrl = response.Url,
                Advertisement = advertisement
            });
        }
    }

    private string GetImageName(string url) => url.Split('/').Last();
}
