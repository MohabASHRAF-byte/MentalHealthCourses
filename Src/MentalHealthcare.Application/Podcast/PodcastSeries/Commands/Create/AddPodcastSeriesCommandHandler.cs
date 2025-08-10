using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Instructors.Commands.Create;
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

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create
{
    public class AddPodcastSeriesCommandHandler(
     ILogger<AddPodcastSeriesCommandHandler> logger,
        IMapper mapper,
        IPodcastSeriesRepository SeriesRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService) : IRequestHandler<AddPodcastSeriesCommand, int>
    {
        public async Task<int> Handle(AddPodcastSeriesCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);


            // Map the request to the Instructor entity
            var NewSeris = mapper.Map<Domain.Entities.PodcastSeries>(request);
            // Assign the Data from the current user
            //     NewSeris.ad = (int)currentUser.AdminId;


            // Upload thumbnail using BunnyCDN
            logger.LogInformation("Uploading Photo For Podcast Series: {PodcastSeriesName}", NewSeris.NameOfTopic);

            var bunny = new BunnyClient(configuration);
            NewSeris.Image = $"{NewSeris.NameOfTopic}.jpeg";

            var imageResponse = await bunny
     .UploadFileAsync(request.Image, NewSeris.NameOfTopic, "PodcastSeries"
            );


            if (!imageResponse.IsSuccessful)
            {
                logger.LogWarning("Failed to upload image for InstructorsId: {InstructorsId}. Error: {ErrorMessage}",
                            request.NameOfTopic, imageResponse.Message);
                throw new BadHttpRequestException(
                     localizationService.GetMessage("FailedToUploadImage", "Failed to upload image. Please try again.")
                 );


            }

            // Clear cache for the uploaded thumbnail
            logger.LogInformation("Clearing cache for the uploaded Photo at URL: {Photo}", imageResponse.Url);
            await bunny.ClearCacheAsync(imageResponse.Url!);

            // Update PodcastSeries Info with the new Image URL
            NewSeris.Image = imageResponse.Url;
            await SeriesRepo.SaveChangesAsync();
            logger.LogInformation("Successfully Uploaded Photo For PodcastSeriesId: {PodcastSeriesId}. URL: {ImageUrl}",
                request.NameOfTopic, NewSeris.Image);

            // Insert the new PodcastSeries into the database
            logger.LogInformation("Inserting New Podcast Series {PodcastSeriesName} into the database", request.NameOfTopic);
            var podcastSeriesId = await SeriesRepo.CreatePodcastSeriesAsync(NewSeris);
            return podcastSeriesId; // ✅ FIX: Ensure a return value


        }
    }
}
