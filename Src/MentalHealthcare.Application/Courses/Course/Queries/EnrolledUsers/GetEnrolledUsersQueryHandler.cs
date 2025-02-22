using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Course.Queries.EnrolledUsers;

public class GetEnrolledUsersQueryHandler(
    ILogger<GetEnrolledUsersQueryHandler> logger,
    IUserContext userContext,
    ICourseRepository courseRepository
) : IRequestHandler<GetEnrolledUsersQuery, PageResult<GetUserProfile>>
{
    public async Task<PageResult<GetUserProfile>> Handle(GetEnrolledUsersQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting handling of GetEnrolledUsersQuery for CourseId: {CourseId}, PageNumber: {PageNumber}, PageSize: {PageSize}", request.CourseId, request.PageNumber, request.PageSize);
        
        try
        {
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User authorization successful.");
            
            var (total, users) = await courseRepository.GetEnrolledUsersAsync(
                request.CourseId,
                request.PageNumber,
                request.PageSize,
                request.Search
            );

            logger.LogInformation("Retrieved {TotalUsers} enrolled users for CourseId: {CourseId}", total, request.CourseId);
            
            return new PageResult<GetUserProfile>(users, total, request.PageSize, request.PageNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving enrolled users for CourseId: {CourseId}", request.CourseId);
            throw;
        }
    }
}