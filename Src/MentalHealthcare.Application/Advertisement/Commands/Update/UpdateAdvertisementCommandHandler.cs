using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Commands.Update;

public class UpdateAdvertisementCommandHandler(
    ILogger<DeleteAdvertisementCommandHandler> logger,
    IAdvertisementRepository adRepository,
    IMapper mapper
    ):IRequestHandler<UpdateAdvertisementCommand, int>
{
    public async Task<int> Handle(UpdateAdvertisementCommand request, CancellationToken cancellationToken)
    {
       logger.LogInformation($"UpdateAdvertisementCommandHandler invoked.");

       if(request.AdvertisementId == null)
           return 0;
       var currAd = await adRepository.GetAdvertisementByIdAsync((int)request.AdvertisementId);
       var remove = new List<string>();
       foreach (var imgUrl in request.ImagesUrls)
       {
           if (currAd.AdvertisementImageUrls.Any(currImgUrl => currImgUrl.ImageUrl == imgUrl))
           {
               continue;
           }
           remove.Add(imgUrl);
       }
       //todo
       // loop delete the image
       // loop upload the new images and push to the add
       await adRepository.UpdateAdvertisementAsync(currAd);
       return currAd.AdvertisementId;

    }
}