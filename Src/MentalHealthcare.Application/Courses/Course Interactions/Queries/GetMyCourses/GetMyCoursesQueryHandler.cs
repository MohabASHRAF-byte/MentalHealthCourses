using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetMyCourses;

public class GetMyCoursesQueryHandler(
    ILogger<GetMyCoursesQueryHandler> logger,
    ICourseInteractionsRepository courseInteractionsRepository,
    IUserContext userContext
) : IRequestHandler<GetMyCoursesQuery, PageResult<CourseActivityDto>>
{
    public async Task<PageResult<CourseActivityDto>> Handle(GetMyCoursesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetMyCoursesQuery for PageNumber: {PageNumber}, PageSize: {PageSize}, SearchTerm: {SearchTerm}",
            request.PageNumber, request.PageSize, request.SearchTerm);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning(
                "Unauthorized access attempt to fetch courses. User information: {UserDetails}",
                currentUser == null
                    ? "User is null"
                    : $"UserId: {currentUser.Id}, Roles: {string.Join(",", currentUser.Roles)}"
            );
            throw new ForBidenException("You do not have permission to fetch courses.");
        }

        logger.LogInformation("Fetching active course progress for UserId: {UserId}", currentUser.SysUserId);

        var (count, courses) = await courseInteractionsRepository
            .GetActiveCourseProgress(
                currentUser.SysUserId!.Value,
                request.PageNumber,
                request.PageSize,
                request.SearchTerm
            );

        logger.LogInformation("Successfully retrieved {Count} courses for UserId: {UserId}", count, currentUser.SysUserId);

        return new PageResult<CourseActivityDto>(
            courses, count, request.PageSize, request.PageNumber);
    }
}