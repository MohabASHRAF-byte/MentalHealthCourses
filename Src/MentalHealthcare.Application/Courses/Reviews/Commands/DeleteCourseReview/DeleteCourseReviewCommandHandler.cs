using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Reviews.Commands.DeleteCourseReview;

public class DeleteCourseReviewCommandHandler(
    ILogger<DeleteCourseReviewCommandHandler> logger,
    ICourseReview courseReview,
    IUserContext userContext
) : IRequestHandler<DeleteCourseReviewCommand>
{
    public async Task Handle(DeleteCourseReviewCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting Handle method for DeleteCourseReviewCommand with CourseId: {CourseId}, ReviewId: {ReviewId}",
            request.CourseId, request.ReviewId);

        var currentUser = userContext.UserHaveAny([UserRoles.Admin,UserRoles.User],logger);
        
        int? userId = null;
        if (currentUser.HasRole(UserRoles.User))
        {
            userId = currentUser.SysUserId!.Value;
            logger.LogInformation(
                "User with UserId: {UserId} is attempting to delete their review for CourseId: {CourseId}",
                userId, request.CourseId);
        }
        else
        {
            logger.LogInformation(
                "Admin is attempting to delete review with ReviewId: {ReviewId} for CourseId: {CourseId}",
                request.ReviewId, request.CourseId);
        }

        try
        {
            await courseReview.DeleteCourseReviewAsync(
                userId,
                request.CourseId,
                request.ReviewId
            );
            logger.LogInformation(
                "Successfully deleted review with ReviewId: {ReviewId} for CourseId: {CourseId}",
                request.ReviewId, request.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete review with ReviewId: {ReviewId} for CourseId: {CourseId}",
                request.ReviewId, request.CourseId);
            throw;
        }
    }
}
