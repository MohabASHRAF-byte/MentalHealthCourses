using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Queries.GetAll;

/// <summary>
/// Processes the query to retrieve all courses based on the provided search criteria and pagination details.
/// </summary>
public class GetAllCoursesQueryHandler(
    ILogger<GetAllCoursesQueryHandler> logger,
    ICourseRepository courseRepository,
    IUserContext userContext
) : IRequestHandler<GetAllCoursesQuery, PageResult<CourseViewDto>>
{
    public async Task<PageResult<CourseViewDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting Handle method for GetAllCoursesQuery. SearchText: {SearchText}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.SearchText, request.PageNumber, request.PageSize);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning("Unauthorized attempt to retrieve courses.");
            throw new ForBidenException("You do not have permission to retrieve courses.");
        }

        logger.LogInformation("User {UserId} is attempting to retrieve courses.", currentUser.Id);

        int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;

        logger.LogDebug("Calling repository to get all courses for UserId: {UserId}", userId);

        // Retrieve all courses from the repository
        var (count, courses) =
            await courseRepository.GetAllAsync(
                userId,
                request.SearchText,
                request.PageNumber,
                request.PageSize
            );

        logger.LogInformation("Retrieved {Count} courses for UserId: {UserId}", count, userId);

        // Map the retrieved courses to DTOs
        logger.LogDebug("Mapping retrieved courses to CourseViewDto.");
        var ret = new PageResult<CourseViewDto>(
            courses,
            count,
            request.PageSize,
            request.PageNumber);

        logger.LogInformation(
            "Successfully processed GetAllCoursesQuery. Returning {TotalCount} courses.",
            count);

        return ret;
    }
}