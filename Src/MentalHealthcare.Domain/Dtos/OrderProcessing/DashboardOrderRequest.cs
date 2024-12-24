using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.OrderProcessing;

public class DashboardOrderRequest
{
    public int InvoiceId { get; set; }

    //
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    //
    public string Notes { get; set; } = string.Empty;
    public int NumberOfItems { set; get; } = 0;
    public string PromoCode { set; get; } = string.Empty;
    public decimal SubTotalPrice { set; get; } = 0;
    public decimal DiscountPercentage { set; get; } = 0;
    public decimal DiscountValue { set; get; } = 0;
    public decimal TaxesPercentage { set; get; } = 0;
    public decimal TaxesValue { set; get; } = 0;
    public decimal TotalPrice { set; get; } = 0;
    public DateTime OrderDate { set; get; } = DateTime.UtcNow;
    public List<CourseOrderView> Courses { set; get; } = [];
    public OrderStatus OrderStatus { set; get; }
}