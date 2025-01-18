using MediatR;
using MentalHealthcare.Application.BunnyServices;
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

namespace MentalHealthcare.Application.Instructors.Commands.AddPhoto
{
    public class AddPhotoCommandHandler(
    ILogger<AddPhotoCommandHandler> logger,
    IInstructorRepository insRepo,
    IMediator mediator,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<AddPhotoCommand, string>
    {
        public async Task<string> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {



            // Retrieve current user and validate permissions
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning(
                    "Unauthorized access attempt to add a Photo For Instructor. User information: {UserDetails}",
                    currentUser == null
                        ? "User is null"
                        : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
                );
                throw new ForBidenException("You do not have permission to add Photo For this Instructor.");
            }




            // Show Instructor Info
            logger.LogInformation("Retrieving Instructor details for InstructorId: {InstructorId}", request.InstructorsId);
            var Ins = await insRepo.GetInstructorByIdAsync(request.InstructorsId);
            if (Ins == null)
            {
                logger.LogError("Instructor with ID {InstructorsId} not found.", request.InstructorsId);
                throw new ResourceNotFound("Instructor", request.InstructorsId.ToString());
            }



            // Upload thumbnail using Bunny service
            logger.LogInformation("Uploading Photo For Instructor: {Name Of Instructor}", Ins.Name);
            var bunny = new BunnyClient(configuration);
            Ins.ImageUrl = $"{Ins.Name}.jpeg";
            var ImageResponse = await bunny.UploadFileAsync(
                request.File, Ins.Name, "InstructorsPhoto"
                );

            if (!ImageResponse.IsSuccessful)
            {
                logger.LogWarning("Failed to upload image for InstructorsId: {InstructorsId}. Error: {ErrorMessage}",
                    request.InstructorsId, ImageResponse.Message);
                throw new TryAgain("Failed to upload Image. Please try again.");
            }


            // Clear cache for the uploaded thumbnail
            logger.LogInformation("Clearing cache for the uploaded Photo at URL: {Photo}", ImageResponse.Url);
            await bunny.ClearCacheAsync(ImageResponse.Url!);


            // Update Instructor Info with the New Image URL
            Ins.ImageUrl = ImageResponse.Url;
            await insRepo.SaveChangesAsync();
            logger.LogInformation("Successfully Photo For InstructorsId: {InstructorsId}. URL: {ImageUrl}",
                request.InstructorsId, Ins.ImageUrl);


            return Ins.ImageUrl!;



































        }
    }
}
