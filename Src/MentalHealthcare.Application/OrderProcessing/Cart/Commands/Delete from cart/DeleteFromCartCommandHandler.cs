using MediatR;
using AutoMapper;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;
using MentalHealthcare.Domain.Exceptions;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Delete_from_cart;

public class DeleteFromCartCommandHandler(
    ILogger<DeleteFromCartCommandHandler> logger,
    IMapper mapper,
    ICartRepository cartRepository,
    ICourseRepository courseRepository,
    IUserContext userContext
) : IRequestHandler<DeleteFromCartCommand>
{
    public async Task Handle(DeleteFromCartCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Delete From Cart Command for user: {UserId}", userContext.GetCurrentUser()?.Id);

        // Validate user context
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning("Unauthorized attempt to delete course from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }

        logger.LogInformation("Attempting to remove course with ID {CourseId} from the cart for user: {UserId}", request.CourseId, currentUser.Id);

        // Use repository method to remove the course from the cart
        await cartRepository.RemoveItemFromCartAsync(
            currentUser.Id
            ,request.CourseId
        );

        logger.LogInformation("Successfully removed course with ID {CourseId} from the cart for user: {UserId}", request.CourseId, currentUser.Id);
    }
}