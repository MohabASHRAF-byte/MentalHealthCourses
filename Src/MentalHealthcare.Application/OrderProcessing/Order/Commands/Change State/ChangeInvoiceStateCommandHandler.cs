using MediatR;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Change_State;

public class ChangeInvoiceStateCommandHandler(
    ILogger<ChangeInvoiceStateCommandHandler> logger,
    IInvoiceRepository invoiceRepository
    ):IRequestHandler<ChangeInvoiceStateCommand>
{
    public async Task Handle(ChangeInvoiceStateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Change invoice state command received");
        
    }
}