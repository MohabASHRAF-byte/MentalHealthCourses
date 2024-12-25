using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Queries.Get_invoice;

public class GetInvoiceQueryHandler(
    ILogger<GetInvoiceQueryHandler> logger,
    IUserContext userContext,
    IInvoiceRepository invoiceRepository
) : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting handling of GetInvoiceQuery for InvoiceId: {InvoiceId}", request.InvoiceId);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning("Unauthorized attempt to access an invoice. InvoiceId: {InvoiceId}", request.InvoiceId);
            throw new ForBidenException("You do not have permission to access this invoice.");
        }

        InvoiceDto invoice;
        int? userId;
        if (currentUser.HasRole(UserRoles.Admin))
        {
            userId = (int?)null;
            logger.LogInformation("Fetching invoice for Admin. AdminId: {UserId}, InvoiceId: {InvoiceId}",
                currentUser.Id, request.InvoiceId);
        }
        else if (currentUser.HasRole(UserRoles.User))
        {
            userId = currentUser.SysUserId!.Value;
            logger.LogInformation("Fetching invoice for User. UserId: {UserId}, InvoiceId: {InvoiceId}", currentUser.Id,
                request.InvoiceId);
        }
        else
        {
            logger.LogWarning("Unexpected role encountered for UserId: {UserId}. Role: {Role}", currentUser.Id,
                currentUser.Roles);
            throw new ForBidenException("Unexpected role. Access denied.");
        }

        invoice = await invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId, userId);

        return invoice;
    }
}