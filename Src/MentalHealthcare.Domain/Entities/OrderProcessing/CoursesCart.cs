namespace MentalHealthcare.Domain.Entities.OrderProcessing;

public class CoursesCart
{
    public int CoursesCartId { get; set; }
    public Guid UserId { get; set; }

    public int ItemsCount { get; set; } = 0;
    public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;
    public List<CartItem> Items { get; set; } = [];
}