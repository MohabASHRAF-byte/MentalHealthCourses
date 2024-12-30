using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;

public class DeleteCourseThumbnailCommandHandler(
    ILogger<DeleteCourseThumbnailCommandHandler> logger,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<DeleteCourseThumbnailCommand>
{
    public async Task Handle(DeleteCourseThumbnailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteCourseThumbnailCommand for CourseId: {CourseId}", request.CourseId);

        // Retrieve current user and validate permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning(
                "Unauthorized access attempt to delete a thumbnail. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to delete a thumbnail for this course.");
        }

        logger.LogInformation("Retrieving course details for CourseId: {CourseId}", request.CourseId);
        var course = await courseRepository.GetMinimalCourseByIdAsync(request.CourseId);
        if (course == null)
        {
            logger.LogError("Course with ID {CourseId} not found.", request.CourseId);
            throw new ResourceNotFound("Course", request.CourseId.ToString());
        }

        if (string.IsNullOrEmpty(course.ThumbnailName))
        {
            logger.LogInformation("Course with ID {CourseId} does not have a thumbnail to delete.", request.CourseId);
            return;
        }

        logger.LogInformation("Deleting thumbnail for CourseId: {CourseId}", request.CourseId);
        var bunny = new BunnyClient(configuration);
        await bunny.DeleteFileAsync(course.ThumbnailName, Global.CourseThumbnailDirectory);

        logger.LogInformation(
            "Thumbnail deleted successfully for CourseId: {CourseId}. Clearing thumbnail information in the database.",
            request.CourseId);
        course.ThumbnailUrl = null;
        await courseRepository.SaveChangesAsync();

        logger.LogInformation("Successfully handled DeleteCourseThumbnailCommand for CourseId: {CourseId}",
            request.CourseId);
    }
}