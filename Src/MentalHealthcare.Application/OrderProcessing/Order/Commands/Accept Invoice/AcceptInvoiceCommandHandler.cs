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
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning("Unauthorized attempt to delete from cart by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to delete items from the cart.");
        }

        await invoiceRepository.AcceptInvoice(
            request.InvoiceId,
            request.Courses,
            request.Discount,
            currentUser.AdminId!.Value
        );
    }
}