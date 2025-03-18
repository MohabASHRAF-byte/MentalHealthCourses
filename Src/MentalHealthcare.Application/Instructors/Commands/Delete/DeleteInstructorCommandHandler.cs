using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Instructors.Commands.Create;
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

namespace MentalHealthcare.Application.Instructors.Commands.Delete
{
    public class DeleteInstructorCommandHandler(
        ILogger<DeleteInstructorCommandHandler> logger,
        IMapper mapper,
        IInstructorRepository insRepo,
        IConfiguration configuration,
        IUserContext userContext
    ) : IRequestHandler<DeleteInstructorCommand>
    {
        public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Delete Instructor");
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);



            var ins = await insRepo.GetInstructorByIdAsync(request.InstructorID);


            if (ins == null)
            {
                logger.LogWarning("Article with ID: {AdId} not found.", request.InstructorID);
                throw new KeyNotFoundException($"Article with ID {request.InstructorID} not found.");
            }


            // Delete the image from BunnyCDN if the ImageUrl is not null
            if (!string.IsNullOrEmpty(ins.ImageUrl))
            {
                var bunny = new BunnyClient(configuration);
                var imgName = GetImageName(ins.ImageUrl);
                await bunny.DeleteFileAsync(imgName, Global.InstructorFolderName);
            }
            else
            {
                logger.LogWarning($"Instructor with ID {request.InstructorID} has no image URL.");
            }

            // Delete the instructor from the database
            await insRepo.DeleteInstructorAsync(request.InstructorID);
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