using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.OrderProcessing;

namespace MentalHealthcare.Domain.Repositories.OrderProcessing;

public interface ICartRepository 
{
    public Task<CoursesCart?> GetCartByUserIdAsync(string userId);
    public Task<int> CreateAsync(CoursesCart coursesCart);
    public Task<bool> IsCourseExistInCartAsync(int courseId , string userId);
    public Task SaveChangesAsync();
    public Task RemoveItemFromCartAsync(string userId, int courseId);

    public Task<IEnumerable<CourseCartDto>> GetCartItemsByUserIdAsync(string userId);

    public Task RemoveCartAsync(string currentUserId);
}