using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Advertisement.Commands.Delete;

public class DeleteAdvertisementCommandHandler(
    ILogger<DeleteAdvertisementCommandHandler> logger,
    IAdvertisementRepository adRepository,
    IConfiguration configuration,
    IUserContext userContext
    ):IRequestHandler<DeleteAdvertisementCommand>
{
    public async Task Handle(DeleteAdvertisementCommand request, CancellationToken cancellationToken)
    {
       logger.LogInformation("Delete advertisement");
       var currentUser = userContext.GetCurrentUser();
       if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
       {
           logger.LogWarning("Unauthorized attempt to delete advertisement by user: {UserId}", currentUser?.Id);
           throw new ForBidenException("Don't have the permission to delete advertisement.");
       }
       var ad = await adRepository.GetAdvertisementByIdAsync(request.AdvertisementId);
       var bunny = new BunnyClient(configuration);
       foreach (var img in ad.AdvertisementImageUrls)
       {
           var imgName = GetImageName(img.ImageUrl);
          await bunny.DeleteFileAsync(imgName,Global.AdvertisementFolderName);
       }
       await adRepository.DeleteAdvertisementAsync(request.AdvertisementId);
    }

    private string GetImageName(string url)
    {
        return url.Split('/').Last();
    }
}