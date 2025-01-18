using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articles.Commands.Update;
using MentalHealthcare.Application.Instructors.Commands.Delete;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MentalHealthcare.Application.BunnyServices;
using System.Net;

namespace MentalHealthcare.Application.Instructors.Commands.Update
{
    public class UpdateInstructorCommandHandler(
     ILogger<UpdateInstructorCommandHandler> logger,
     IMapper mapper,
     IInstructorRepository insRepo,
     IConfiguration configuration,
     IUserContext userContext
 ) : IRequestHandler<UpdateInstructorCommand, int>
    {
        public async Task<int> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning(
                    "Unauthorized access attempt to update an Info of Instructor. User information: {UserDetails}",
                    currentUser == null
                        ? "User is null"
                        : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
                );
                throw new ForBidenException("You do not have permission to update an Info of Instructor.");
            }


            // Validate instructor ID
            if (request.instructorid == null)
            {
                logger.LogWarning("Instructor ID is null. Aborting update process.");
                return 0;
            }


            // Update Instructor details
            logger.LogInformation("Updating details for Instructor ID: {instructorid}.", request.instructorid);
            //Fetching Instructor  From DB
            logger.LogInformation("Fetching Instructor with ID: {instructorid}."
                , request.instructorid);
            var ins = await insRepo.GetInstructorByIdAsync((int)request.instructorid);


            if (ins == null)
            {
                logger.LogError("Instructor with ID {instructorid} not found.",
                    request.instructorid);
                throw new KeyNotFoundException($"Instructor with ID {request.instructorid} not found.");
            }



            // Update Instructor details
            logger.LogInformation("Updating details for Instructor ID: {instructorid}.",
                request.instructorid);
            UpdateInstructorInfo(ref ins, request);

         
  var bunnyClient = new BunnyClient(configuration);
            // Handle existing images
            logger.LogInformation("Handling existing Photo for Instructor ID: {Instructor}.", request.instructorid);
            HandleExistingImages(ref ins, request, bunnyClient);



            // Upload new images
            logger.LogInformation("Uploading new images for Instructor ID: {Instructor}.", request.instructorid);
            UploadNewImages(ref ins, request, bunnyClient);




            await insRepo.UpdateInstructorAsync(ins);

            logger.LogInformation("Instructor ID: {InstructorId} updated successfully.", request.instructorid);
            return ins.InstructorId;

        }

        private void UploadNewImages(ref Domain.Entities.Instructor ins, UpdateInstructorCommand request, BunnyClient bunnyClient)
        {


            var newImageName = $"{ins.InstructorId}_{DateTime.UtcNow}.jpeg";

       var response = bunnyClient.UploadFileAsync(request.File, newImageName, Global.InstructorFolderName).Result;

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning("Failed to upload new image for Instructor ID: {InstructorId}. Error: {Message}",
                    ins.InstructorId, response.Message ?? "Unknown error");
                throw new Exception("Failed to upload the new image.");
            }


            ins.ImageUrl = response.Url;
            logger.LogInformation("Successfully uploaded new image for Instructor ID: {InstructorId}. URL: {Url}",
                ins.InstructorId, response.Url);

        }

        private void HandleExistingImages(ref Domain.Entities.Instructor ins, UpdateInstructorCommand request, BunnyClient bunnyClient)
        {
            // Case 1: Admin provides a new photo


            if (request.File != null)
            {

                //TODO : Delete the old photo if it exists
                if (!string.IsNullOrEmpty(ins.ImageUrl))
                {
                    var oldImageName = GetImageName(ins.ImageUrl);
                     bunnyClient.DeleteFileAsync(oldImageName, Global.InstructorFolderName).Wait();
                    logger.LogInformation("Deleted old image for Instructor ID: {InstructorId}."
                        , ins.InstructorId);

                    ins.ImageUrl = null;

                }

              
            }

            // Case 2: Admin does not change the photo (no new photo or URL provided)
            else
            {
                logger.LogInformation("No changes made to the photo for Instructor ID: {InstructorId}. Retaining the existing photo.",
                    ins.InstructorId);
            }


        }





        private void UpdateInstructorInfo(ref Domain.Entities.Instructor instructor, UpdateInstructorCommand request)
        {
            if (!request.Name.IsNullOrEmpty())
            {
                instructor.Name = request.Name!;
                logger.LogInformation("Updated Name for Instructor ID: {instructorid} to {Name}."
                        , instructor.InstructorId, request.Name);
            }

            if (!request.About.IsNullOrEmpty())
            {
                instructor.About = request.About!;
                logger.LogInformation("Updated About for Instructor ID: {instructorid} to {About}."
                    , instructor.InstructorId, request.About);
            }
        }



        private string GetImageName(string url) => url.Split('/').Last();

    }
}









