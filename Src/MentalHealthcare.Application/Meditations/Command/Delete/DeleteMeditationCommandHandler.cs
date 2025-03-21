using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Instructors.Commands.Delete;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Delete
{
    public class DeleteMeditationCommandHandler(

          ILogger<CreateMeditationCommandHandler> logger,
        IMapper mapper,
        IMeditationRepository MedRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService


        ) : IRequestHandler<DeleteMeditationCommand>
    {
        public async Task Handle(DeleteMeditationCommand request, CancellationToken cancellationToken)
        {



            logger.LogInformation("Delete Meditation");
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);



            var med = await MedRepo.GetMeditationByIdAsync(request.MeditationID);


            if (med == null)
            {
                logger.LogWarning("Meditation with ID: {AdId} not found.", request.MeditationID);
                throw new KeyNotFoundException($"Article with ID {request.MeditationID} not found.");
            }


            // Delete the image from BunnyCDN if the ImageUrl is not null
            if (!string.IsNullOrEmpty(med.Image))
            {
                var bunny = new BunnyClient(configuration);
                var imgName = GetImageName(med.Image);
                await bunny.DeleteFileAsync(imgName, Global.MeditationFolderName);
            }
            else
            {
                logger.LogWarning($"Instructor with ID {request.MeditationID} has no image URL.");
            }

            // Delete the instructor from the database
            await MedRepo.DeleteMeditationAsync(request.MeditationID);



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
