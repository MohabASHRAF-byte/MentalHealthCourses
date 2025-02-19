using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Reviews.Commands.AddCourseReview;

public class AddCourseReviewCommandHandler(
    ILogger<AddCourseReviewCommandHandler> logger,
    ICourseReview courseReview,
    IUserContext userContext
) : IRequestHandler<AddCourseReviewCommand, int>
{
    public async Task<int> Handle(AddCourseReviewCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting Handle method for AddCourseReviewCommand with CourseId: {CourseId}, Rating: {Rating}",
            request.CourseId, request.Rating);
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);

        logger.LogInformation("User with SysUserId: {SysUserId} is adding a review for CourseId: {CourseId}",
            currentUser.SysUserId, request.CourseId);

        var review = new UserReview()
        {
            courseId = request.CourseId,
            SystemUserId = currentUser.SysUserId!.Value,
            Rating = (float)Math.Round(request.Rating, 1), // Round rating to nearest digit
            Content = request.Content,
            ReviewDate = DateTime.UtcNow,
        };

        try
        {
            await courseReview.AddCourseReviewAsync(review);
            logger.LogInformation(
                "Successfully added review with UserReviewId: {UserReviewId} for CourseId: {CourseId}",
                review.UserReviewId, request.CourseId);
            return review.UserReviewId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add review for CourseId: {CourseId} by UserId: {UserId}",
                request.CourseId, currentUser.SysUserId);
            throw;
        }
    }
}