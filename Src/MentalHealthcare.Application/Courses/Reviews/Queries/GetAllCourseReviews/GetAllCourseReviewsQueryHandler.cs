using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Reviews.Queries.GetAllCourseReviews;

public class GetAllCourseReviewsQueryHandler(
    ILogger<GetAllCourseReviewsQueryHandler> logger,
    ICourseReview courseReview,
    IUserContext userContext
) : IRequestHandler<GetAllCourseReviewsQuery, PageResult<UserReviewDto>>
{
    public async Task<PageResult<UserReviewDto>> Handle(GetAllCourseReviewsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling GetAllCourseReviewsQuery for CourseId: {CourseId}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.CourseId, request.PageNumber, request.PageSize);

        userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);

        try
        {
            var (count, reviews) = await courseReview.GetCoursesReviewsAsync(
                request.CourseId,
                request.PageNumber,
                request.PageSize,
                request.ContentLimit
            );

            logger.LogInformation("Successfully retrieved {ReviewCount} reviews for CourseId: {CourseId}",
                count, request.CourseId);

            return new PageResult<UserReviewDto>(
                reviews,
                count,
                request.PageSize,
                request.PageNumber
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve reviews for CourseId: {CourseId}", request.CourseId);
            throw;
        }
    }
}