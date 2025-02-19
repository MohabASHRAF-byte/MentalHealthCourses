using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
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
        userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);

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