using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Reviews.Queries.GetCourseReview;

public class GetCourseReviewQueryHandler(
    ILogger<GetCourseReviewQueryHandler> logger,
    ICourseReview courseReview,
    IUserContext userContext
) : IRequestHandler<GetCourseReviewQuery, UserReviewDto>
{
    public async Task<UserReviewDto> Handle(GetCourseReviewQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetCourseReviewQuery for CourseId: {CourseId}, ReviewId: {ReviewId}", 
            request.CourseId, request.ReviewId);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning(
                "Unauthorized access attempt to retrieve course reviews. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to retrieve reviews for this course.");
        }

        try
        {
            var review = await courseReview.GetUserReviewAsync(
                request.CourseId,
                request.ReviewId
            );

            logger.LogInformation("Successfully retrieved review with ReviewId: {ReviewId} for CourseId: {CourseId}", 
                request.ReviewId, request.CourseId);

            return review;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve review with ReviewId: {ReviewId} for CourseId: {CourseId}", 
                request.ReviewId, request.CourseId);
            throw;
        }
    }
}