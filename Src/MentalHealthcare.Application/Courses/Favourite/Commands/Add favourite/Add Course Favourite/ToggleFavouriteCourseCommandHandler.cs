using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Favourite.Commands.Add_favourite.Add_Course_Favourite;

public class ToggleFavouriteCourseCommandHandler(
    ILogger<ToggleFavouriteCourseCommandHandler> logger,
    IUserContext userContext,
    ICourseFavouriteRepository favouriteRepository
    
    ): IRequestHandler<ToggleFavouriteCourseCommand>
{
    public async Task Handle(ToggleFavouriteCourseCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning("Unauthorized attempt to delete from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }



       await favouriteRepository.ToggleFavouriteCourseAsync(request.CourseId, currentUser.Id);

    }
}