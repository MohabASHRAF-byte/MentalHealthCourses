using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Update_Resource;

public class UpdateLessonResourceCommandHandler(
    ILogger<UpdateLessonResourceCommandHandler> logger,
    ICourseResourcesRepository courseResourcesRepository,
    IUserContext userContext
) : IRequestHandler<UpdateLessonResourceCommand, int>
{
    public async Task<int> Handle(UpdateLessonResourceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UpdateLessonResourceCommand for Resource ID: {ResourceId} and Lesson ID: {LessonId}", 
            request.ResourceId, request.LessonId);

        // Authenticate and validate admin permissions
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            var userDetails = currentUser == null
                ? "User is null"
                : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}";

            logger.LogWarning("Unauthorized access attempt to update resource. User details: {UserDetails}", userDetails);
            throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        logger.LogInformation("Admin access granted for user {UserId}", currentUser.Id);

        // Fetch the resource to update
        var resource = await courseResourcesRepository.GetCourseLessonResourceByIdAsync(request.ResourceId);
        if (resource == null)
        {
            logger.LogWarning("Resource with ID {ResourceId} not found.", request.ResourceId);
            throw new KeyNotFoundException($"Resource with ID {request.ResourceId} not found.");
        }

        logger.LogInformation("Updating resource title for Resource ID: {ResourceId}", request.ResourceId);

        // Update resource properties
        resource.Title = request.ResourceName;

        // Save changes to the repository
        await courseResourcesRepository.SaveChangesAsync();

        logger.LogInformation("Successfully updated resource with ID: {ResourceId}. New Title: {ResourceName}", 
            resource.CourseLessonResourceId, request.ResourceName);

        return resource.CourseLessonResourceId;
    }
}
