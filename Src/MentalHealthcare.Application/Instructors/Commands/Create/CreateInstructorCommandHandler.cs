using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Courses.Course.Commands.Create;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MentalHealthcare.Application.Resources.Localization.Resources;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Instructors.Commands.Create
{
    public class CreateInstructorCommandHandler(
        ILogger<CreateInstructorCommandHandler> logger,
        IMapper mapper,
        IInstructorRepository insRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService
    ) : IRequestHandler<CreateInstructorCommand, CreateInstructorCommandResponse>
    {
        public async Task<CreateInstructorCommandResponse> Handle(CreateInstructorCommand request,
            CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);


            // Map the request to the Instructor entity
            var newInstructor = mapper.Map<Domain.Entities.Instructor>(request);
            // Assign the Data from the current user
            newInstructor.AddedByAdminId = (int)currentUser.AdminId;
            //newInstructor.Name = request.Name;
            //newInstructor.About = request.About;
            //wait insRepo.CreateInstructorAsync(newInstructor);


            // Upload thumbnail using Bunny service
            logger.LogInformation("Uploading Photo For Instructor: {Name Of Instructor}", newInstructor.Name);
            var bunny = new BunnyClient(configuration);
            newInstructor.ImageUrl = $"{newInstructor.Name}.jpeg";
            var ImageResponse = await bunny.UploadFileAsync(
                request.File, newInstructor.Name, "Instructors"
            );

            if (!ImageResponse.IsSuccessful)
            {
                logger.LogWarning("Failed to upload image for InstructorsId: {InstructorsId}. Error: {ErrorMessage}",
                    request.Name, ImageResponse.Message);
                throw new BadHttpRequestException(
                    localizationService.GetMessage("FailedToUploadImage", "Failed to upload image. Please try again.")
                );
            }


            // Clear cache for the uploaded thumbnail
            logger.LogInformation("Clearing cache for the uploaded Photo at URL: {Photo}", ImageResponse.Url);
            await bunny.ClearCacheAsync(ImageResponse.Url!);


            // Update Instructor Info with the New Image URL
            newInstructor.ImageUrl = ImageResponse.Url;
            await insRepo.SaveChangesAsync();
            logger.LogInformation("Successfully Photo For InstructorsId: {InstructorsId}. URL: {ImageUrl}",
                request.Name, newInstructor.ImageUrl);


            // Insert the new instructor into the database
            logger.LogInformation("Inserting New Instructor {InstructorName} into the database", request.Name);
            var instructorId = await insRepo.CreateInstructorAsync(newInstructor);


            // Return the response
            return new CreateInstructorCommandResponse
            {
                InstructorId = instructorId
            };
        }
    }
}