using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning("Unauthorized attempt to delete from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }

        var (count, courses) = await favouriteRepository.GetUserFavourites(
            currentUser.Id,
            request.PageNumber,
            request.PageSize,
            request.Search ?? ""
        );

        return new PageResult<CourseViewDto>(
            courses,
            count,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
        );
    }
}