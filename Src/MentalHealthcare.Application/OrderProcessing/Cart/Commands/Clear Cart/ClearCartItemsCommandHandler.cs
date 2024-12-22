using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;

public class ClearCartItemsCommandHandler(
    ILogger<ClearCartItemsCommandHandler> logger,
    IMapper mapper,
    ICartRepository cartRepository,
    ICourseRepository courseRepository,
    IUserContext userContext
    ):IRequestHandler<ClearCartItemsCommand>
{
    public async Task Handle(ClearCartItemsCommand request, CancellationToken cancellationToken)
    {
        // Validate user context
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning("Unauthorized attempt to delete from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }

        cartRepository.RemoveCartAsync(currentUser.Id);
    }
}