using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Commands.AddIcon;

public class AddCourseIconCommandHandler(
    ILogger<AddCourseIconCommandHandler> logger,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<AddCourseIconCommand>
{
    public async Task Handle(AddCourseIconCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling AddCourseIconCommand for CourseId: {CourseId}", request.CourseId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to add an icon. User details: {UserDetails}", userDetails);
            throw new ForBidenException("You do not have permission to add an icon to this course.");
        }

        logger.LogInformation("User {UserId} authorized to add course icon.", currentUser.Id);

        // Retrieve course information
        logger.LogInformation("Fetching course details for CourseId: {CourseId}", request.CourseId);
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.CourseId);
        if (course == null)
        {
            logger.LogError("Course with ID {CourseId} not found.", request.CourseId);
            throw new ResourceNotFound("Course", request.CourseId.ToString());
        }

        logger.LogInformation("Course found: {CourseName} (ID: {CourseId})", course.Name, request.CourseId);

        // Upload icon using Bunny service
        logger.LogInformation("Uploading icon for course: {CourseName}", course.Name);
        var bunny = new BunnyClient(configuration);
        course.IconName = $"{course.Name}_Icon.jpeg";

        var thumbnailResponse = await bunny.UploadFileAsync(
            request.File, course.IconName, Global.CourseIconsDirectory);

        if (!thumbnailResponse.IsSuccessful)
        {
            logger.LogWarning("Failed to upload icon for CourseId: {CourseId}. Error: {ErrorMessage}", 
                request.CourseId, thumbnailResponse.Message);
            throw new TryAgain("Failed to upload icon. Please try again.");
        }

        logger.LogInformation("Icon uploaded successfully for CourseId: {CourseId}. URL: {ThumbnailUrl}", 
            request.CourseId, thumbnailResponse.Url);

        // Clear cache for the uploaded icon
        logger.LogInformation("Clearing cache for uploaded icon at URL: {ThumbnailUrl}", thumbnailResponse.Url);
        await bunny.ClearCacheAsync(thumbnailResponse.Url!);

        // Update course with the icon URL
        course.IconUrl = thumbnailResponse.Url;
        await courseRepository.SaveChangesAsync();

        logger.LogInformation("Successfully updated CourseId: {CourseId} with IconUrl: {IconUrl}", 
            request.CourseId, course.IconUrl);

        logger.LogInformation("End handling AddCourseIconCommand for CourseId: {CourseId}", request.CourseId);

    }
}
