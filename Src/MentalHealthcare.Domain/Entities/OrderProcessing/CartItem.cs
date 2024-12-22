namespace MentalHealthcare.Domain.Entities.OrderProcessing;

public class CartItem
{
    public int CartItemId { get; set; }
    public int CoursesCartId { get; set; }
    public CoursesCart Cart { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    

}