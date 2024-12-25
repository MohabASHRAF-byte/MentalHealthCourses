using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Dtos.OrderProcessing;

public class InvoiceViewDto
{
    public int InvoiceId { get; set; }
    public int NumberOfItems { set; get; } = 0;
    public decimal TotalPrice { set; get; } = 0;
    public DateTime OrderDate { set; get; } = DateTime.UtcNow;
    public OrderStatus OrderStatus { set; get; }
    public string AdminName { set; get; }=string.Empty;
    public string Name { set; get; } = string.Empty;
    public string Email { set; get; } = string.Empty;
    public string Phone { set; get; } = string.Empty;
    public string PromoCode { set; get; } = string.Empty;
    
}