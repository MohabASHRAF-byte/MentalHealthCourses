using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Commands.Create;

/*
 * the problem is that we want to generate a new id for each img and keep that generation valid for the feature updates
 *  solution
 *    make a cnt for the number of uploaded images and for each img upload increase the cnt by 1
 * that will grant the uniqueness of the element for each advertisment
 * and to make unique for all ads combine the id of the advertisement with that cnt
 *
 */
/*
 * - check the size of the photos be
 * - create the new ad
 * - get last uploaded img id
 */
public class CreateAdvertisementCommandHandler(
    ILogger<CreateAdvertisementCommandHandler> logger,
    IAdvertisementRepository adRepository,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<CreateAdvertisementCommand, int>
{
    public async Task<int> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(@"Creating advertisement with name {ad}", request.AdvertisementName);
        //todo
        // add auth

        CheckPhotosSize(ref request);
        var newAd = mapper.Map<Domain.Entities.Advertisement>(request);
        await adRepository.CreateAdvertisementAsync(newAd);
        var bunny = new BunnyClient(configuration);
        foreach (var img in request.Images)
        {
            var newImageName = $"{newAd.AdvertisementId}_{newAd.LastUploadImgCnt}.jpeg";
            newAd.LastUploadImgCnt++;
            var response = await bunny.UploadFile(img, newImageName, Global.AdvertisementFolderName);
            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning(@"Could not upload advertisement {ad} error msg :{mg}", request.AdvertisementName,
                    response.Message ?? ""
                );
                continue;
            }

            newAd.AdvertisementImageUrls.Add(new AdvertisementImageUrl()
            {
                ImageUrl = response.Url,
                Advertisement = newAd
            });
        }

        await adRepository.UpdateAdvertisementAsync(newAd);
        return newAd.AdvertisementId;
    }

    private void CheckPhotosSize(ref CreateAdvertisementCommand request)
    {
        foreach (var img in request.Images)
        {
            var imgSizeInMb = img.Length / (1 << 20);
            if (imgSizeInMb > Global.AdvertisementImgSize)
            {
                logger.LogWarning($"try to upload img with size {imgSizeInMb} ");
                throw new Exception($"Image size cannot be greater than {Global.AdvertisementImgSize} MB");
            }
        }
    }
}
