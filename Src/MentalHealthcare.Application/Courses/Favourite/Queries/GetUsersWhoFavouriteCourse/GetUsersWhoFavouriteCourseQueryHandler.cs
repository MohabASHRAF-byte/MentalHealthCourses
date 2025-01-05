using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Favourite.Queries.GetUsersWhoFavouriteCourse;

public class GetUsersWhoFavouriteCourseQueryHandler(
    ILogger<GetUsersWhoFavouriteCourseQueryHandler> logger,
    IUserContext userContext,
    ICourseFavouriteRepository favouriteRepository
) : IRequestHandler<GetUsersWhoFavouriteCourseQuery, PageResult<SystemUser>>
{
    public async Task<PageResult<SystemUser>> Handle(GetUsersWhoFavouriteCourseQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetUsersWhoFavouriteCourseQuery for Course ID: {CourseId}", request.CourseId);

        // Ensure the user is authorized
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to retrieve users who favourited Course ID: {CourseId}",
            currentUser.Id, request.CourseId);

        try
        {
            logger.LogInformation(
                "Fetching users who favourited Course ID: {CourseId}, Page Number: {PageNumber}, Page Size: {PageSize}, Search Term: {SearchTerm}",
                request.CourseId, request.PageNumber, request.PageSize, request.SearchTerm);

            var (count, users) = await favouriteRepository
                .GetUsersWhoFavouriteCourseAsync(
                    request.CourseId,
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm ?? ""
                );

            logger.LogInformation("Successfully fetched {Count} users who favourited Course ID: {CourseId}", count,
                request.CourseId);

            return new PageResult<SystemUser>(
                users,
                count,
                request.PageSize,
                request.PageNumber
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching users who favourited Course ID: {CourseId}",
                request.CourseId);
            throw;
        }
    }
}