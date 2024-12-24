using MediatR;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Change_State;

public class ChangeInvoiceStateCommand:IRequest
{
    public int InvoiceId { get; set; }
}