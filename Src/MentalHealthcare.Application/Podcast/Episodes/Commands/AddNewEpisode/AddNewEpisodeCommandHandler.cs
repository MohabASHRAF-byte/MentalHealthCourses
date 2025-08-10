using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Commands.AddNewEpisode
{
    public class AddNewEpisodeCommandHandler(
     ILogger<AddNewEpisodeCommandHandler> logger,
        IMapper mapper,
        IPodcastRepository PodRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService) : IRequestHandler<AddNewEpisodeCommand, int>
    {
        public async Task<int> Handle(AddNewEpisodeCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);


            var newPodcast = mapper.Map<Domain.Entities.Podcast>(request);
            newPodcast.UploadedById = (int)currentUser.AdminId;



            logger.LogInformation("Uploading Audio For Episode: {Name Of Instructor}", newPodcast.Title);
            var bunny = new BunnyClient(configuration);
            newPodcast.AudioFile = $"{newPodcast.Title}.mp3";

            var AudioResponse = await bunny.UploadFileAsync(
              request.AudioFileUpload, newPodcast.Title, "Podcasts"
          );

            if (!AudioResponse.IsSuccessful)
            {
                logger.LogWarning("Failed to upload Audio for PodcastsId: {PodcastsId}. Error: {ErrorMessage}",
                    request.Title, AudioResponse.Message);
                throw new BadHttpRequestException(
                    localizationService.GetMessage("FailedToUploadImage", "Failed to upload image. Please try again.")
                );
            }








            logger.LogInformation("Clearing cache for the uploaded Audio at URL: {Photo}", AudioResponse.Url);
            await bunny.ClearCacheAsync(AudioResponse.Url!);


            newPodcast.AudioFile = AudioResponse.Url;
            await PodRepo.SaveChangesAsync();
            logger.LogInformation("Successfully Audio For PodcastsId: {PodcastsId}. URL: {PodcastsId}",
                request.Title, newPodcast.AudioFile);


            // Insert the new instructor into the database
            logger.LogInformation("Inserting New Instructor {InstructorName} into the database", request.Title);
            var podcastid = await PodRepo.AddNewEpisodeAsync(newPodcast);


            // Return the response
            return podcastid;
            







        }
    }
}
