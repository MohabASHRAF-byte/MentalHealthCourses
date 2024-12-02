using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Commands.Delete;

public class DeleteAdvertisementCommandHandler(
    ILogger<DeleteAdvertisementCommandHandler> logger,
    IAdvertisementRepository adRepository,
    IConfiguration configuration
    ):IRequestHandler<DeleteAdvertisementCommand>
{
    public async Task Handle(DeleteAdvertisementCommand request, CancellationToken cancellationToken)
    {
       logger.LogInformation("Delete advertisement");
       var ad = await adRepository.GetAdvertisementByIdAsync(request.AdvertisementId);
       var bunny = new BunnyClient(configuration);
       foreach (var img in ad.AdvertisementImageUrls)
       {
           var imgName = GetImageName(img.ImageUrl);
          await bunny.DeleteFile(imgName,Global.AdvertisementFolderName);
       }
       await adRepository.DeleteAdvertisementAsync(request.AdvertisementId);
    }

    private string GetImageName(string url)
    {
        return url.Split('/').Last();
    }
}