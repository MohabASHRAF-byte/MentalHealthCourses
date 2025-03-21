using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Meditations.Command.Create;
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

namespace MentalHealthcare.Application.Meditations.Command.Update
{
    public class UpdateMeditationCommandHandler(ILogger<CreateMeditationCommandHandler> logger,
        IMapper mapper,
        IMeditationRepository MedRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService
) : IRequestHandler<UpdateMeditationCommand, int>
    {
        public async Task<int> Handle(UpdateMeditationCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

            // Validate meditation ID
            if (request.Meditationid <= 0)
            {
                logger.LogWarning("Invalid Meditation ID: {MeditationId}. Aborting update process.", request.Meditationid);
                throw new ArgumentException(localizationService.GetMessage("InvalidMeditationId"));
            }

            // Fetch meditation from database
            logger.LogInformation("Fetching Meditation with ID: {MeditationId}.", request.Meditationid);
            var meditation = await MedRepo.GetMeditationByIdAsync(request.Meditationid);

            if (meditation == null)
            {
                logger.LogError("Meditation with ID {MeditationId} not found.", request.Meditationid);
                throw new ResourceNotFound(
                    "Meditation",
                    "تأمل",
                    request.Meditationid.ToString()
                );
            }

            // Update Meditation details
            UpdateMeditationInfo(ref meditation, request);

            var bunnyClient = new BunnyClient(configuration);

            if (request.Image != null)
            {
                HandleExistingImage(ref meditation, bunnyClient);
                UploadNewImage(ref meditation, request.Image, bunnyClient);
            }

            // Save changes to repository
            await MedRepo.UpdateMeditationAsync(meditation);
            logger.LogInformation("Meditation ID: {MeditationId} updated successfully.", request.Meditationid);

            return meditation.MeditationId; }



        private void UpdateMeditationInfo(ref Meditation meditation, UpdateMeditationCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                meditation.Title = request.Title;
                logger.LogInformation("Updated Title for Meditation ID: {MeditationId} to {Title}.", meditation.MeditationId, request.Title);
            }

            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                meditation.Content = request.Content;
                logger.LogInformation("Updated Content for Meditation ID: {MeditationId}.", meditation.MeditationId);
            }
        }

        private void HandleExistingImage(ref Meditation meditation, BunnyClient bunnyClient)
        {
            if (!string.IsNullOrEmpty(meditation.Image))
            {
                var oldImageName = GetImageName(meditation.Image);

                try
                {
                    bunnyClient.DeleteFileAsync(oldImageName, Global.MeditationFolderName).Wait();
                    logger.LogInformation("Deleted old image for Meditation ID: {MeditationId}.", meditation.MeditationId);
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to delete old image for Meditation ID: {MeditationId}. Error: {Error}", meditation.MeditationId, ex.Message);
                    throw new BadHttpRequestException(localizationService.GetMessage("OldImageDeletionError"));
                }
            }
        }





        private void UploadNewImage(ref Meditation meditation, IFormFile image, BunnyClient bunnyClient)
        {
            var newImageName = $"{meditation.MeditationId}.jpeg";

            var response = bunnyClient.UploadFileAsync(image, newImageName, Global.MeditationFolderName).Result;

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning("Failed to upload new image for Meditation ID: {MeditationId}. Error: {Message}", meditation.MeditationId, response.Message ?? "Unknown error");
                throw new BadHttpRequestException(localizationService.GetMessage("ImageUploadFailed"));
            }

            meditation.Image = response.Url;
            logger.LogInformation("Successfully uploaded new image for Meditation ID: {MeditationId}. URL: {Url}", meditation.MeditationId, response.Url);
        }

        private string GetImageName(string url) => url.Split('/').Last();
    }







}

