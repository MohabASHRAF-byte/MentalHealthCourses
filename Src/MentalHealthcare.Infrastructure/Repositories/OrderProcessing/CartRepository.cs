using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories.OrderProcessing;

public class CartRepository(
    MentalHealthDbContext dbContext
) : ICartRepository
{
    public async Task<CoursesCart?> GetCartByUserIdAsync(string userId)
    {
        return await dbContext.Carts
            .Include(cart => cart.Items)
            .FirstOrDefaultAsync(cart => cart.UserId.ToString() == userId);
    }

    public async Task<int> CreateAsync(CoursesCart coursesCart)
    {
        await dbContext.Carts.AddAsync(coursesCart);
        await dbContext.SaveChangesAsync();
        return coursesCart.CoursesCartId;
    }

    public async Task<bool> IsCourseExistInCartAsync(int courseId, string userId)
    {
        return await dbContext.CartItems
            .AnyAsync(item => item.CourseId == courseId &&
                              dbContext.Carts.Any(
                                  cart => cart.UserId.ToString() == userId && cart.CoursesCartId == item.CoursesCartId));
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveItemFromCartAsync(string userId, int courseId)
    {
        // Fetch the cart for the user
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId.ToString() == userId);

        if (cart == null)
        {
            throw new ArgumentException("No cart found for the user.");
        }

        // Check if the item exists in the cart
        var cartItem = cart.Items.FirstOrDefault(item => item.CourseId == courseId);
        if (cartItem == null)
        {
            throw new ArgumentException("The course is not in your cart.");
        }

        // Remove the item from the cart and mark it for deletion
        dbContext.CartItems.Remove(cartItem);
        cart.LastUpdatedDate = DateTime.UtcNow;
        cart.ItemsCount -= 1;

        // Save changes to the database
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseCartDto>> GetCartItemsByUserIdAsync(string userId)
    {
        var cartItems = await dbContext.Carts
            .Where(c => c.UserId.ToString() == userId)
            .SelectMany(c => c.Items)
            .Select(item => new CourseCartDto
            {
                CourseId = item.CourseId,
                Name = item.Course.Name,
                ThumbnailUrl = item.Course.ThumbnailUrl,
                Price = item.Course.Price,
                Description = item.Course.Description
            })
            .ToListAsync();

        if (!cartItems.Any())
        {
          return new List<CourseCartDto>();
        }

        return cartItems;
    }

    public async Task RemoveCartAsync(string currentUserId)
    {
        // Fetch the cart for the user
        var cart = await dbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId.ToString() == currentUserId);

        if (cart == null)
        {
            throw new ArgumentException("No cart found for the user.");
        }

        // Remove the cart (with cascade delete ensuring cart items are also removed)
        dbContext.Carts.Remove(cart);

        // Save changes to the database
        await dbContext.SaveChangesAsync();
    }
}