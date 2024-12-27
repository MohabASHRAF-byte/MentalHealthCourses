using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
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
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning(
                "Unauthorized access attempt to add a course review. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to add a review for this course.");
        }

        var (count, reviews) = await courseReview.GetCoursesReviewsAsync(
            request.CourseId,
            request.PageNumber,
            request.PageSize,
            request.ContentLimit
        );

        return new PageResult<UserReviewDto>(
            reviews,
            count,
            request.PageSize,
            request.PageNumber
        );
    }
}