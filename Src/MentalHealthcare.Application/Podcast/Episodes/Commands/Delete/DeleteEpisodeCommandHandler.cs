using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Delete;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Commands.Delete
{
    public class DeleteEpisodeCommandHandler(
        ILogger<DeleteEpisodeCommandHandler> logger,
        IMapper mapper,
        IPodcastRepository PodRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService) : IRequestHandler<DeleteEpisodeCommand>
    {
        public async Task Handle(DeleteEpisodeCommand request, CancellationToken cancellationToken)
        {



            logger.LogInformation("Delete Episode");
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);



            var pod = await PodRepo.GetEpisodeByIdAsync(request.EpisodeId);


            if (pod == null)
            {
                logger.LogWarning("Episode with ID: {EpisodeId} not found.", request.EpisodeId);
                throw new ResourceNotFound("Podcast Episode", "الحلقة غير موجودة", request.EpisodeId.ToString());
            }


            // Delete the audio file from BunnyCDN (if exists)
            if (!string.IsNullOrWhiteSpace(pod.AudioFile))
            {
                try
                {
                    var bunny = new BunnyClient(configuration);
                    var audioFileName = GetFileNameFromUrl(pod.AudioFile);
                    await bunny.DeleteFileAsync(audioFileName, Global.PodcastSeriesFolderName);
                    logger.LogInformation("Deleted audio file: {FileName} from BunnyCDN", audioFileName);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error deleting audio file from BunnyCDN for Episode ID: {EpisodeId}", request.EpisodeId);
                }
            }

            // Delete the instructor from the database
            await PodRepo.DeleteEpisodeAsync(request.EpisodeId);




        }



        private string GetFileNameFromUrl(string url)
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
