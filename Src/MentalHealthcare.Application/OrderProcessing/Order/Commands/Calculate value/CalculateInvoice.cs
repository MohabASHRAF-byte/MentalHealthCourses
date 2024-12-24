using MediatR;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Calculate_value;

public class CalculateInvoiceResponse
{
    public List<InvoiceCourse> Result { get; set; } = new();
    public int NumberOfItems { set; get; } = 0;
    public decimal SubTotalPrice { set; get; } = 0;
    public decimal DiscountPercentage { set; get; } = 0;
    public decimal DiscountValue { set; get; } = 0;
    public decimal TaxesPercentage { set; get; } = 0;
    public decimal TaxesValue { set; get; } = 0;
    public decimal TotalPrice { set; get; } = 0;
}

public class InvoiceCourse
{
    public int CourseId { get; set; }
    public decimal Price { get; set; }
}

public class CalculateInvoice : IRequest<CalculateInvoiceResponse>
{
    public List<InvoiceCourse> Courses { get; set; } = [];
    public decimal DiscountPercentage { get; set; }
    public decimal TaxPercentage { get; set; }
}