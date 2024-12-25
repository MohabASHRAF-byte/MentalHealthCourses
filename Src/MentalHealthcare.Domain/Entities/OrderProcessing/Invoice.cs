using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;

namespace MentalHealthcare.Domain.Entities.OrderProcessing;

public class Invoice
{
    public int InvoiceId { get; set; }

    //
    public int SystemUserId { get; set; }
    public SystemUser SystemUser { get; set; }

    public string UserID { get; set; }
    public User User { get; set; }

    //
    public int? AdminId { get; set; }

    public Admin? Admin { get; set; }

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

    public DateTime ProcessedDate { set; get; } = DateTime.UtcNow;
}