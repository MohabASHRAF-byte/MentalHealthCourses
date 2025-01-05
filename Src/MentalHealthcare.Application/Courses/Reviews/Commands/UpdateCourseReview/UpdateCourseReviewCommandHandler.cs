using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Reviews.Commands.UpdateCourseReview;

public class UpdateCourseReviewCommandHandler(
    ILogger<UpdateCourseReviewCommandHandler> logger,
    ICourseReview courseReview,
    IUserContext userContext
) : IRequestHandler<UpdateCourseReviewCommand>
{
    public async Task Handle(UpdateCourseReviewCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting Handle method for UpdateCourseReviewCommand with CourseId: {CourseId}, ReviewId: {ReviewId}, Rating: {Rating}",
            request.CourseId, request.ReviewId, request.Rating);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to update a course review. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to update this review.");
        }

        if (request.Content is null && request.Rating is null)
        {
            logger.LogInformation("No updates provided for ReviewId: {ReviewId}, skipping update.", request.ReviewId);
            return;
        }

        try
        {
            await courseReview.UpdateCourseReviewAsync(
                currentUser.SysUserId!.Value,
                request.CourseId,
                request.ReviewId,
                request.Rating,
                request.Content
            );
            logger.LogInformation("Successfully updated ReviewId: {ReviewId} for CourseId: {CourseId}", request.ReviewId, request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update ReviewId: {ReviewId} for CourseId: {CourseId}", request.ReviewId, request.CourseId);
            throw;
        }
    }
}
