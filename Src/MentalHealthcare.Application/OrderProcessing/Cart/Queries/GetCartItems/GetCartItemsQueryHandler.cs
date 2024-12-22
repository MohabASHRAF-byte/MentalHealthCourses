using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;

public class GetCartItemsQueryHandler(
    ILogger<GetCartItemsQueryHandler> logger,
    IMapper mapper,
    ICartRepository cartRepository,
    ICourseRepository courseRepository,
    IUserContext userContext
) : IRequestHandler<GetCartItemsQuery, IEnumerable<CourseCartDto>>
{
    public async Task<IEnumerable<CourseCartDto>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Add to Cart Command handling for user.");

        // Validate user context
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning("Unauthorized attempt to add to cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to add items to the cart.");
        }

        logger.LogInformation("Fetching cart for user: {UserId}", currentUser.Id);

        // Fetch or initialize the user's cart
        var cart = await cartRepository.GetCartByUserIdAsync(currentUser.Id);
        if (cart == null)
        {
            logger.LogInformation("Cart not found for user: {UserId}", currentUser.Id);
            return new List<CourseCartDto>();
        }

        var cartItems = await cartRepository.GetCartItemsByUserIdAsync(currentUser.Id);
        return cartItems;
    }
}