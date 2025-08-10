using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using System.Net;
using MentalHealthcare.Application.Instructors.Commands.Update;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Update
{
    public class UpdatePodcastSeriesCommandHandler(
        ILogger<UpdatePodcastSeriesCommandHandler> logger,
        IPodcastSeriesRepository SeriesRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService
    ) : IRequestHandler<UpdatePodcastSeriesCommand, int>
    {
        public async Task<int> Handle(UpdatePodcastSeriesCommand request, CancellationToken cancellationToken)
        {
            // Authorize user
            logger.LogInformation("Authorizing user for updating podcast series.");
            var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
            logger.LogInformation("User {UserId} authorized to update podcast series.", currentUser.Id);

            // Validate Podcast Series ID
            if (request.PodcastSeriesId == 0)
            {
                logger.LogWarning("Podcast Series ID is invalid. Aborting update process.");
                throw new ResourceNotFound("PodcastSeries", "السلسلة غير موجودة", request.PodcastSeriesId.ToString());
            }

            // Retrieve podcast series
            logger.LogInformation("Fetching podcast series with ID: {SeriesId}.", request.PodcastSeriesId);
            var podcastSeries = await SeriesRepo.GetPodcastSeriesByIdAsync(request.PodcastSeriesId);

            if (podcastSeries == null)
            {
                logger.LogWarning("Podcast series with ID: {SeriesId} not found.", request.PodcastSeriesId);
                throw new ResourceNotFound("PodcastSeries", "السلسلة غير موجودة", request.PodcastSeriesId.ToString());
            }
        //    podcastSeries.upll = currentUser.Id; // Assign the logged-in user


            // Update Series details
            logger.LogInformation("Updating details for Series ID: {SeriesId}.",
                request.PodcastSeriesId);
            UpdateSeriesInfo(ref podcastSeries, request);
            var bunnyClient = new BunnyClient(configuration);

            if (request.Image!= null)
            {
                logger.LogInformation("Handling existing Photo for Serirs ID: {Series}.", request.PodcastSeriesId);
                HandleExistingImages(ref podcastSeries, request, bunnyClient);


                // Upload new images
                logger.LogInformation("Uploading new images for Instructor ID: {Instructor}.", request.PodcastSeriesId);
                UploadNewImages(ref podcastSeries, request, bunnyClient);
            }


            //foreach (var podcast in podcastSeries.Podcasts)
            //{
            //    if (podcast.UploadedBy == null)  // Example of handling missing values
            //    {
            //        podcast.UploadedBy =  dcurrentUser.Id; // Set a default value or handle appropriately
            //    }
            //}

            await SeriesRepo.UpdatePodcastSeriesAsync(podcastSeries);

            logger.LogInformation("Instructor ID: {InstructorId} updated successfully.", request.PodcastSeriesId);
            return podcastSeries.PodcastSeriesId;

        }

        private void UploadNewImages(ref Domain.Entities.PodcastSeries Series, UpdatePodcastSeriesCommand request, BunnyClient bunnyClient)
        {

            var newImageName = $"{Series.PodcastSeriesId}.jpeg";

            var response = bunnyClient.UploadFileAsync(request.Image, newImageName, Global.PodcastSeriesFolderName).Result;

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning("Failed to upload new image for PodcastSeries ID: {PodcastSeriesID}. Error: {Message}",
                    Series.PodcastSeriesId, response.Message ?? "Unknown error");
                throw new BadHttpRequestException(
                    localizationService.GetMessage("ImageUploadFailed")
                );
            }


            Series.Image = response.Url;
            logger.LogInformation("Successfully uploaded new image for PodcastSeries ID: {PodcastSeriesID}. URL: {Url}",
                Series.PodcastSeriesId, response.Url);



        }

        private void HandleExistingImages(ref  Domain.Entities.PodcastSeries Series, UpdatePodcastSeriesCommand request, BunnyClient bunnyClient)
        {



            // Case 1: Admin provides a new photo
            if (request.Image != null)
            {
                // Delete the old photo if it exists
                if (!string.IsNullOrEmpty(Series.Image))
                {
                    var oldImageName = GetImageName(Series.Image);

                    try
                    {
                        bunnyClient.DeleteFileAsync(oldImageName, Global.PodcastSeriesFolderName).Wait();
                        logger.LogInformation("Deleted old image for podcastSeries ID: {podcastSeriesId}.", Series.PodcastSeriesId);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Failed to delete old image for podcastSeries ID: {podcastSeriesId}. Error: {Error}",
                            Series.PodcastSeriesId, ex.Message);
                        throw new BadHttpRequestException(
                            localizationService.GetMessage("OldImageDeletionError")
                        );
                    }
                }




                else
                {
                    logger.LogInformation("No existing image to delete for podcastSeries ID: {InstructorId}.",
                        Series.PodcastSeriesId);
                }
            }



            else
            {
                logger.LogInformation(
                    "No changes made to the photo for Instructor ID: {InstructorId}. Retaining the existing photo.",
                    Series.PodcastSeriesId);
        



            }
        }
        private void UpdateSeriesInfo(ref Domain.Entities.PodcastSeries Series , UpdatePodcastSeriesCommand request)
        {
            if (!request.NameOfTopic.IsNullOrEmpty())
            {
                Series.NameOfTopic = request.NameOfTopic!;
                logger.LogInformation("Updated Name for Instructor ID: {instructorid} to {Name}."
                    , Series.PodcastSeriesId, request.NameOfTopic);
            }
            else
            {
                logger.LogInformation("No new Name provided. Retaining existing Name for Series ID: {SeriesID}.",
             Series.PodcastSeriesId);
            }

            if (!string.IsNullOrEmpty(request.PodcastDescription))
            {
                Series.PodcastDescription = request.PodcastDescription!;
                logger.LogInformation("Updated Description for Series ID: {SeriesID} to {PodcastDescription}.",
                    Series.PodcastSeriesId, request.PodcastDescription);
            }
            else
            {
                logger.LogInformation("No new Description provided. Retaining existing Description for Series ID: {SeriesID}.",
                    Series.PodcastSeriesId);
            }


            if (!string.IsNullOrEmpty(request.PodcasterName))
            {
                Series.PodcasterName = request.PodcasterName!;
                logger.LogInformation("Updated Podcaster Name for Series ID: {SeriesID} to {PodcasterName}.",
                    Series.PodcastSeriesId, request.PodcasterName);
            }
            else
            {
                logger.LogInformation("No new Podcaster Name provided. Retaining existing Podcaster Name for Series ID: {SeriesID}.",
                    Series.PodcastSeriesId);
            }




        }
        private string GetImageName(string url) => url.Split('/').Last();












    }
}
