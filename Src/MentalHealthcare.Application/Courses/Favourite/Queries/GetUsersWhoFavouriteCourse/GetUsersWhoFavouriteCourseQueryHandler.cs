// using MediatR;
// using MentalHealthcare.Application.Common;
// using MentalHealthcare.Application.SystemUsers;
// using MentalHealthcare.Domain.Constants;
// using MentalHealthcare.Domain.Entities;
// using MentalHealthcare.Domain.Exceptions;
// using MentalHealthcare.Domain.Repositories.Course;
// using Microsoft.Extensions.Logging;
//
// namespace MentalHealthcare.Application.Courses.Favourite.Queries.GetUsersWhoFavouriteCourse;
//
// public class GetUsersWhoFavouriteCourseQueryHandler(
//     ILogger<GetUsersWhoFavouriteCourseQueryHandler> logger,
//     IUserContext userContext,
//     ICourseFavouriteRepository favouriteRepository
//     ):IRequestHandler<GetUsersWhoFavouriteCourseQuery,PageResult<SystemUser>>
// {
//     public async Task<PageResult<SystemUser>> Handle(GetUsersWhoFavouriteCourseQuery request, CancellationToken cancellationToken)
//     {
//         var currentUser = userContext.GetCurrentUser();
//         if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
//         {
//             logger.LogWarning("Unauthorized attempt to access favourite courses by user: {UserId}", currentUser?.Id);
//             throw new ForBidenException("You do not have permission to access favourite courses.");
//         }
//         
//         
//         
//     }
// }