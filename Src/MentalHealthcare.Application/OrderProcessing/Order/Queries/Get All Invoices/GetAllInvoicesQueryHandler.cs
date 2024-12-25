using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Queries.Get_All_Invoices;

public class GetAllInvoicesQueryHandler(
    ILogger<GetAllInvoicesQueryHandler> logger,
    IUserContext userContext,
    IInvoiceRepository invoiceRepository
) : IRequestHandler<GetAllInvoicesQuery, PageResult<InvoiceViewDto>>
{
    public async Task<PageResult<InvoiceViewDto>> Handle(GetAllInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning("Unauthorized access attempt to fetch all invoices.");
            throw new ForBidenException("You do not have permission to access invoices.");
        }

        logger.LogInformation("Fetching invoices for user: {UserId} with role: {UserRole}", currentUser.Id,
            currentUser.Roles);

        var userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.Id;

        var (count, invoices) = await invoiceRepository.GetInvoicesAsync(
            request.PageNumber, request.PageSize,
            request.InvoiceId,
            request.Status,  request.Email,
            request.PhoneNumber, request.FromDate, request.ToDate,
            request.PromoCode, userId
        );

        logger.LogInformation("Retrieved {InvoiceCount} invoices for user: {UserId}", invoices.Count, currentUser.Id);

        if (currentUser.HasRole(UserRoles.User))
        {
            logger.LogInformation("Redacting sensitive information for user: {UserId}", currentUser.Id);
            foreach (var invoice in invoices)
            {
                invoice.Name = "";
                invoice.Email = "";
                invoice.Phone = "";
            }
        }

        logger.LogInformation("Successfully fetched invoices for page {PageNumber} with page size {PageSize}",
            request.PageNumber, request.PageSize);

        return new PageResult<InvoiceViewDto>(
            invoices,
            count,
            request.PageSize,
            request.PageNumber
        );
    }
}