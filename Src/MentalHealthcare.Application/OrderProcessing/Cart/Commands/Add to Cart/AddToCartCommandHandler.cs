using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Domain.Repositories.Course;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Add_to_Cart;

public class AddToCartCommandHandler(
    ILogger<AddToCartCommandHandler> logger,
    ICartRepository cartRepository,
    ICourseRepository courseRepository,
    IUserContext userContext
) : IRequestHandler<AddToCartCommand, int>
{
    public async Task<int> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Add to Cart Command handling for user.");

        // Validate user context
        var currentUser = userContext
            .EnsureAuthorizedUser(
                [UserRoles.User],
                logger
            );
        logger.LogInformation("Fetching cart for user: {UserId}", currentUser.Id);

        // Fetch or initialize the user's cart
        var cart = await cartRepository.GetCartByUserIdAsync(currentUser.Id);
        if (cart == null)
        {
            logger.LogInformation("No existing cart found for user: {UserId}. Initializing a new cart.",
                currentUser.Id);
            cart = new CoursesCart()
            {
                UserId = Guid.Parse(currentUser.Id),
            };
            await cartRepository.CreateAsync(cart);
        }

        // Check if the cart has reached its item limit
        if (cart.Items.Count >= Global.MaxCartItems)
        {
            logger.LogWarning("Cart item limit reached for user: {UserId}. Maximum allowed items: {MaxItems}",
                currentUser.Id, Global.MaxCartItems);
            throw new ArgumentException("The cart has reached its maximum item capacity.");
        }

        var isEnrolled = await courseRepository
            .IsEnrolledInCourse(request.CourseId, currentUser.SysUserId!.Value);
        if (isEnrolled)
        {
            logger.LogWarning("You already Joint the course");
            throw new ArgumentException("You already Joint the course");
        }

        // Check if the course is already in the cart
        logger.LogInformation("Checking if course {CourseId} is already in the cart for user: {UserId}",
            request.CourseId, currentUser.Id);
        var isCourseInCart = await cartRepository.IsCourseExistInCartAsync(request.CourseId, currentUser.Id);
        if (isCourseInCart)
        {
            logger.LogWarning("Course {CourseId} is already in the cart for user: {UserId}", request.CourseId,
                currentUser.Id);
            throw new ArgumentException("The course is already in your cart.");
        }

        // Check if the course exists
        logger.LogInformation("Validating if course exists: {CourseId}", request.CourseId);
        var courseExists = await courseRepository.DoesCourseExist(request.CourseId);
        if (!courseExists)
        {
            logger.LogWarning("Course does not exist: {CourseId}", request.CourseId);
            throw new ArgumentException("The selected course does not exist.");
        }

        // Add the course to the cart
        logger.LogInformation("Adding course {CourseId} to cart for user: {UserId}", request.CourseId, currentUser.Id);
        var cartItem = new CartItem()
        {
            CourseId = request.CourseId,
            CoursesCartId = cart.CoursesCartId
        };
        cart.Items.Add(cartItem);
        cart.LastUpdatedDate = DateTime.UtcNow;
        cart.ItemsCount += 1;
        // Save changes
        logger.LogInformation("Saving updated cart for user: {UserId}", currentUser.Id);
        await cartRepository.SaveChangesAsync();

        logger.LogInformation("Successfully added course {CourseId} to cart for user: {UserId}", request.CourseId,
            currentUser.Id);
        return cart.CoursesCartId;
    }
}