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
            logger.LogWarning("Unauthorized attempt to delete from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }

        logger.LogInformation("Attempting to remove the cart for user: {UserId}", currentUser.Id);

        // Use repository method to remove the cart
        await cartRepository.RemoveCartAsync(currentUser.Id);

        logger.LogInformation("Successfully removed the cart for user: {UserId}", currentUser.Id);
    }
}