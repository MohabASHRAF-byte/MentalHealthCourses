namespace MentalHealthcare.Domain.Dtos.OrderProcessing;

public class CartDto
{
    public IEnumerable<CourseCartDto> Courses { set; get; } = [];
    public List<string> Messages { set; get; } = new();
    public int NumberOfItems { set; get; } = 0;
    public decimal SubTotalPrice { set; get; } = 0;
    public decimal DiscountPercentage { set; get; } = 0;
    public decimal DiscountValue { set; get; } = 0;
    public decimal TaxesPercentage { set; get; } = 0;
    public decimal TaxesValue { set; get; } = 0;
    public decimal TotalPrice { set; get; } = 0;
}