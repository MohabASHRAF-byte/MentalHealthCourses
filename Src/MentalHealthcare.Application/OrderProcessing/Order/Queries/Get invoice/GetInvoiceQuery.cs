using MediatR;
using MentalHealthcare.Domain.Dtos.OrderProcessing;

namespace MentalHealthcare.Application.OrderProcessing.Order.Queries.Get_invoice;

public class GetInvoiceQuery : IRequest<InvoiceDto>
{
    public int InvoiceId { get; set; }
}