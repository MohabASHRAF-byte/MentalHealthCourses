using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Delete
{
    public class DeletePodcastSeriesCommandHandler(
           ILogger<DeletePodcastSeriesCommandHandler> logger,
        IMapper mapper,
        IPodcastSeriesRepository SeriesRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService) : IRequestHandler<DeletePodcastSeriesCommand>
    {
        public async Task Handle(DeletePodcastSeriesCommand request, CancellationToken cancellationToken)
        {



            logger.LogInformation("Delete Series");
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);



            var ser = await SeriesRepo.GetPodcastSeriesByIdAsync(request.SeriesId);


            if (ser == null)
            {
                logger.LogWarning("Series with ID: {SeriesId} not found.", request.SeriesId);
                throw new KeyNotFoundException($"Series with ID {request.SeriesId} not found.");
            }




            // Delete the image from BunnyCDN if the ImageUrl is not null
            if (!string.IsNullOrEmpty(ser.Image))
            {
                var bunny = new BunnyClient(configuration);
                var imgName = GetImageName(ser.Image);
                await bunny.DeleteFileAsync(imgName, Global.PodcastSeriesFolderName);
            }
            else
            {
                logger.LogWarning($"Series with ID {request.SeriesId} has no image URL.");
            }

            // Delete the instructor from the database
            await SeriesRepo.DeletePodcastSeriesAsync(request.SeriesId);



        }



        private string GetImageName(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                logger.LogWarning("Image URL is null or empty.");
                return null; // or throw an exception, depending on your requirements
            }

            return url.Split('/').Last();
        }


    }
}
