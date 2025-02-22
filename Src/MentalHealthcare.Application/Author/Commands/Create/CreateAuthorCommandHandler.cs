using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Author.Commands.Create;

public class CreateAuthorCommandHandler(
    ILogger<CreateAuthorCommandHandler> logger,
    IMapper mapper,
    IAuthorRepository authRepo,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<CreateAuthorCommand, CreateAuthorCommandResponse>
{
    public async Task<CreateAuthorCommandResponse> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        
        
         // Retrieve the current user
                    var currentUser = userContext.GetCurrentUser();
                    if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
                    {
                        logger.LogWarning(
                            "Unauthorized access attempt to add a Photo. User information: {UserDetails}",
                            currentUser == null
                                ? "User is null"
                                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
                        );
                        throw new ForBidenException("You do not have permission to add a Photo to this Author.");
                    } 

        
        
                    // Map the request to the Instructor entity
                    var newAuthor = mapper.Map<Domain.Entities.Author>(request);
                    // Assign the Data from the current user
                    newAuthor.AddedByAdminId = (int)currentUser.AdminId;
        
        
        
          // Upload thumbnail using Bunny service
                    logger.LogInformation("Uploading Photo For Author: {Name Of Author}", newAuthor.Name);
                    var bunny = new BunnyClient(configuration);
                    newAuthor.ImageUrl = $"{newAuthor.Name}.jpeg";
                    var ImageResponse = await bunny.UploadFileAsync(
                        request.File, newAuthor.Name, "Authors"
                        );
        
                    if (!ImageResponse.IsSuccessful)
                    {
                        logger.LogWarning("Failed to upload image for AuthorId: {AuthorId}. Error: {ErrorMessage}",
                            request.Name, ImageResponse.Message);
                        throw new TryAgain("Failed to upload Image. Please try again.");
                    }
        
        
        
        
                    // Clear cache for the uploaded thumbnail
                    logger.LogInformation("Clearing cache for the uploaded Photo at URL: {Photo}", ImageResponse.Url);
                    await bunny.ClearCacheAsync(ImageResponse.Url!);


                    // Update Author Info with the New Image URL
                    newAuthor.ImageUrl = ImageResponse.Url;
                    await authRepo.SaveChangesAsync();
                    logger.LogInformation("Successfully Photo For AuthorId: {AuthorId}. URL: {ImageUrl}",
                        request.Name, newAuthor.ImageUrl);

        
        
                    // Insert the new instructor into the database
                    logger.LogInformation("Inserting New Instructor {InstructorName} into the database", request.Name);
                    var authorid = await authRepo.CreateAuthorAsync(newAuthor);

          

                    // Return the response
                    return new CreateAuthorCommandResponse
                    {
                        AuId = authorid
                    };


        
        
        
        
        
    }
}