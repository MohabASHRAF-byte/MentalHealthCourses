using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddThumbnail;

public class AddCourseThumbnailCommandHandler(
    ILogger<AddCourseThumbnailCommandHandler> logger,
    ICourseRepository courseRepository,
    IMediator mediator,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<AddCourseThumbnailCommand, string>
{
    public async Task<string> Handle(AddCourseThumbnailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling AddCourseThumbnailCommand for CourseId: {CourseId}", request.CourseId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning(
                "Unauthorized access attempt to add a thumbnail. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to add a thumbnail to this course.");
        }

        // Retrieve course information
        logger.LogInformation("Retrieving course details for CourseId: {CourseId}", request.CourseId);
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.CourseId);
        if (course == null)
        {
            logger.LogError("Course with ID {CourseId} not found.", request.CourseId);
            throw new ResourceNotFound("Course", request.CourseId.ToString());
        }

        // Upload thumbnail using Bunny service
        logger.LogInformation("Uploading thumbnail for course: {CourseName}", course.Name);
        var bunny = new BunnyClient(configuration);
        course.ThumbnailName = $"{course.Name}.jpeg";
        var thumbnailResponse = await bunny.UploadFileAsync(
            request.File, course.ThumbnailName, "CoursesThumbnail"
            );

        if (!thumbnailResponse.IsSuccessful)
        {
            logger.LogWarning("Failed to upload thumbnail image for CourseId: {CourseId}. Error: {ErrorMessage}", 
                request.CourseId, thumbnailResponse.Message);
            throw new TryAgain("Failed to upload thumbnail. Please try again.");
        }

        // Clear cache for the uploaded thumbnail
        logger.LogInformation("Clearing cache for the uploaded thumbnail at URL: {ThumbnailUrl}", thumbnailResponse.Url);
        await bunny.ClearCacheAsync(thumbnailResponse.Url!);

        // Update course with the thumbnail URL
        course.ThumbnailUrl = thumbnailResponse.Url;
        await courseRepository.SaveChangesAsync();
        logger.LogInformation("Successfully added thumbnail for CourseId: {CourseId}. URL: {ThumbnailUrl}", 
            request.CourseId, course.ThumbnailUrl);

        return course.ThumbnailUrl!;
    }
}
