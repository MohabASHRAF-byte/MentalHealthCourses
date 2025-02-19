using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Accept_Invoice;

public class AcceptInvoiceCommandHandler(
    ILogger<AcceptInvoiceCommandHandler> logger,
    IInvoiceRepository invoiceRepository,
    IUserContext userContext
) : IRequestHandler<AcceptInvoiceCommand>
{
    public async Task Handle(AcceptInvoiceCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        await invoiceRepository.AcceptInvoice(
            request.InvoiceId,
            request.Courses,
            request.Discount,
            currentUser.AdminId!.Value
        );
    }
}