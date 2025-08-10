using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Podcast.Episodes.Commands.Delete;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Commands.Update
{
    public class UpdateEpisodesCommandHandler(
        ILogger<UpdateEpisodesCommandHandler> logger,
        IMapper mapper,
        IPodcastRepository PodRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService) : IRequestHandler<UpdateEpisodesCommand, int>
    {
        public async Task<int> Handle(UpdateEpisodesCommand request, CancellationToken cancellationToken)
        {


            // Authorize user
            logger.LogInformation("Authorizing user for updating Episode .");
            var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
            logger.LogInformation("User {UserId} authorized to update Episode.", currentUser.Id);

            // Validate Podcast Series ID
            if (request.PodcastId == 0)
            {
                logger.LogWarning("Episode ID is invalid. Aborting update process.");
                throw new ResourceNotFound("Episode", "السلسلة غير موجودة", request.PodcastId.ToString());
            }

            // Retrieve podcast series
            logger.LogInformation("Fetching Episode with ID: {SeriesId}.", request.PodcastId);
            var Episode = await PodRepo.GetEpisodeByIdAsync(request.PodcastId);

            if (Episode == null)
            {
                logger.LogWarning("Episode with ID: {EpisodeId} not found.", request.PodcastId);
                throw new ResourceNotFound("EpisodeId", "الحلقه غير موجودة", request.PodcastId.ToString());
            }





            // Update Series details
            logger.LogInformation("Updating Episode for Episode ID: {EpisodeId}.",
                request.PodcastId);
            UpdateEpisodeInfo(ref Episode, request);
            var bunnyClient = new BunnyClient(configuration);

            if (request.AudioFile != null)
            {
                logger.LogInformation("Handling existing Photo for Episode ID: {EpisodeId}.", request.PodcastId);
                HandleExistingAudioFilesAsync(ref Episode, request, bunnyClient);


                // Upload new images
                logger.LogInformation("Uploading new images for Episode ID: {EpisodeId}.", request.PodcastId);
                UploadNewAudioFiles(ref Episode, request, bunnyClient);
            }



            await PodRepo.UpdateEpisodeAsync(Episode);
            await PodRepo.SaveChangesAsync();
            logger.LogInformation("Episode ID: {EpisodeId} updated successfully.", request.PodcastId);
            return Episode.PodcastId;}

        private void UploadNewAudioFiles(ref Domain.Entities.Podcast episode, UpdateEpisodesCommand request, BunnyClient bunnyClient)
        { var newImageName = $"{episode.PodcastId}.jpeg";

            var response = bunnyClient.UploadFileAsync(request.AudioFile, newImageName, Global.PodcastFolderName).Result;

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning("Failed to upload new Audio File for Episode ID: {EpisodeId}. Error: {Message}",
                    episode.PodcastId, response.Message ?? "Unknown error");
     throw new BadHttpRequestException(
         localizationService.GetMessage("AudioFileUploadFailed")
                );
            } episode.AudioFile = response.Url;
            logger.LogInformation("Successfully uploaded new Audio File for Episode ID: {EpisodeId}. URL: {Url}",
                episode.PodcastId, response.Url); }

        private void HandleExistingAudioFilesAsync(ref Domain.Entities.Podcast episode, UpdateEpisodesCommand request, BunnyClient bunnyClient)
        {


            // Case 1: Admin provides a new AudioFile
            if (request.AudioFile != null)
            {
                // Delete the old photo if it exists
                if (!string.IsNullOrEmpty(episode.AudioFile))
                {
                    var oldImageName = GetFileNameFromUrl(episode.AudioFile);

                    try
                    {
                        bunnyClient.DeleteFileAsync(oldImageName, Global.PodcastFolderName).Wait();
                        logger.LogInformation("Deleted old image for podcast ID: {EpisodeId}.", episode.PodcastId);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Failed to delete old image for podcast ID: {EpisodeId}. Error: {Error}",
                            episode.PodcastId, ex.Message);
                        throw new BadHttpRequestException(
                            localizationService.GetMessage("OldEpisodeDeletionError")
                        );
                    }
                }




                else
                {
                    logger.LogInformation("No existing image to delete for podcast ID: {EpisodeId}.",
                        episode.PodcastId);
                }
            }



            else
            {
                logger.LogInformation(
                    "No changes made to the photo for Episode ID: {EpisodeId}. Retaining the existing Audio File.",
                     episode.PodcastId);




            }
        }








        private string GetFileNameFromUrl(string url)
        {
            return url?.Split('/').Last() ?? string.Empty;
        }






        private void UpdateEpisodeInfo(ref Domain.Entities.Podcast episode, UpdateEpisodesCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                episode.Title = request.Title;
                logger.LogInformation("Updated Title for Episode ID: {EpisodeId} to {Title}.", episode.PodcastId, request.Title);
            }

            if (request.EpisodeLength > 0)
            {
                episode.EpisodeLength = request.EpisodeLength;
                logger.LogInformation("Updated Episode Length for Episode ID: {EpisodeId} to {EpisodeLength}.", episode.PodcastId, request.EpisodeLength);
            }
        }




















    }

}
