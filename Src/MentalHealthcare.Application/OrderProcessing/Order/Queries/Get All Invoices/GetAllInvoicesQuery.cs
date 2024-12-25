using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;

namespace MentalHealthcare.Application.OrderProcessing.Order.Queries.Get_All_Invoices;

public class GetAllInvoicesQuery : IRequest<PageResult<InvoiceViewDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public string? InvoiceId { get; set; } = string.Empty;
    public OrderStatus? Status { get; set; } = null;
    public string? Name { set; get; } = null;
    public string? Email { set; get; } = null;
    public string? PhoneNumber { set; get; } = null;
    public DateTime? ToDate { set; get; } = null;
    public DateTime? FromDate { set; get; } = null;
    public string? PromoCode { set; get; } = null;
}