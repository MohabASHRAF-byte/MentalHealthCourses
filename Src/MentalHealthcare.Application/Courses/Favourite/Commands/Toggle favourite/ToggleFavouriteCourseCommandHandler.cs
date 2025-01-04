using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Favourite.Commands.Toggle_favourite;

public class ToggleFavouriteCourseCommandHandler(
    ILogger<ToggleFavouriteCourseCommandHandler> logger,
    IUserContext userContext,
    ICourseFavouriteRepository favouriteRepository
) : IRequestHandler<ToggleFavouriteCourseCommand>
{
    public async Task Handle(ToggleFavouriteCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ToggleFavouriteCourseCommand for Course ID: {CourseId}", request.CourseId);

        // Ensure the user is authorized
        var currentUser = userContext.EnsureAuthorizedUser(
            [UserRoles.User], logger
        );
        logger.LogInformation("User {UserId} authorized to toggle favourite for Course ID: {CourseId}",
            currentUser.SysUserId, request.CourseId);

        // Perform the toggle favourite operation
        try
        {
            logger.LogInformation("Toggling favourite status for Course ID: {CourseId} by User ID: {UserId}",
                request.CourseId, currentUser.SysUserId);
            await favouriteRepository.ToggleFavouriteCourseAsync(
                request.CourseId,
                currentUser.SysUserId!.Value
            );
            logger.LogInformation("Successfully toggled favourite status for Course ID: {CourseId}", request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while toggling favourite status for Course ID: {CourseId} by User ID: {UserId}",
                request.CourseId, currentUser.SysUserId);
            throw;
        }
    }
}