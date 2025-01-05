using System.Text.Json.Serialization;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Dtos.OrderProcessing;

public class InvoiceDto
{
    public int InvoiceId { get; set; }

    //
    [JsonIgnore]
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
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