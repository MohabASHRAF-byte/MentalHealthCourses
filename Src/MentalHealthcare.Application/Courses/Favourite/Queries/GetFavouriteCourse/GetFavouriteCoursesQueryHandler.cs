using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Favourite.Queries.GetFavouriteCourse;

public class GetFavouriteCoursesQueryHandler(
    ILogger<GetFavouriteCoursesQueryHandler> logger,
    IUserContext userContext,
    ICourseFavouriteRepository favouriteRepository
) : IRequestHandler<GetFavouriteCoursesQuery, PageResult<CourseViewDto>>
{
    public async Task<PageResult<CourseViewDto>> Handle(GetFavouriteCoursesQuery request,
        CancellationToken cancellationToken)
    {
        // Log start of the process
        logger.LogInformation(
            "Handling GetFavouriteCoursesQuery for PageNumber: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            request.PageNumber, request.PageSize, request.Search);

        var currentUser = userContext.EnsureAuthorizedUser(
            [UserRoles.User], logger
        );

        try
        {
            // Fetch user favourites
            var (count, courses) = await favouriteRepository.GetUserFavourites(
                currentUser.SysUserId!.Value,
                request.PageNumber,
                request.PageSize,
                request.Search ?? ""
            );

            // Log successful fetch
            logger.LogInformation("Fetched {CourseCount} courses out of {TotalCount} for user {UserId}",
                courses.Count, count, currentUser.Id);

            return new PageResult<CourseViewDto>(
                courses,
                count,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching favourite courses for user {UserId}", currentUser.Id);
            throw;
        }
    }
}