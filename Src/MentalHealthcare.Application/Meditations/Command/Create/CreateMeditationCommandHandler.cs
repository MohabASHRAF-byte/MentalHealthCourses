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

namespace MentalHealthcare.Application.Meditations.Command.Create
{
    public class CreateMeditationCommandHandler(
         ILogger<CreateMeditationCommandHandler> logger,
        IMapper mapper,
        IMeditationRepository MedRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService




        ) : IRequestHandler<CreateMeditationCommand, CreateMeditationCommandResponse>
    {
        async Task<CreateMeditationCommandResponse> IRequestHandler<CreateMeditationCommand, CreateMeditationCommandResponse>.Handle(CreateMeditationCommand request, CancellationToken cancellationToken)
        {

            // Retrieve the current user
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);


            // Map the request to the Instructor entity
            var newMeditation = mapper.Map<Domain.Entities.Meditation>(request);
            // Assign the Data from the current user
            newMeditation.UploadedById = (int)currentUser.AdminId;


            // Upload thumbnail using Bunny service
            logger.LogInformation("Uploading Photo For Meditation: {Name Of Instructor}", newMeditation.Title);
            var bunny = new BunnyClient(configuration);
            newMeditation.Image = $"{newMeditation.Title}.jpeg";
            var ImageResponse = await bunny.UploadFileAsync(
                request.Image, newMeditation.Title, "Meditations"
            );


            if (!ImageResponse.IsSuccessful)
            {
                logger.LogWarning("Failed to upload image for MeditationId : {MeditationId}. Error: {ErrorMessage}",
                    request.Title, ImageResponse.Message);
                throw new BadHttpRequestException(
                    localizationService.GetMessage("FailedToUploadImage", "Failed to upload image. Please try again.")
                );
            }


            // Clear cache for the uploaded thumbnail
            logger.LogInformation("Clearing cache for the uploaded Photo at URL: {Photo}", ImageResponse.Url);
            await bunny.ClearCacheAsync(ImageResponse.Url!);


            // Update Instructor Info with the New Image URL
            newMeditation.Image = ImageResponse.Url;
            await MedRepo.SaveChangesAsync();
            logger.LogInformation("Successfully Photo For InstructorsId: {InstructorsId}. URL: {ImageUrl}",
                request.Title, newMeditation.Image);

            // Insert the new instructor into the database
            logger.LogInformation("Inserting New Instructor {InstructorName} into the database", request.Title);
            var meditationId = await MedRepo.CreateMeditationAsync(newMeditation);


            // Return the response
            return new CreateMeditationCommandResponse
            {
                MeditationId = meditationId
            };
        }
    }
}
