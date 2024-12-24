using AutoMapper;
using MediatR;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Place;

public class PlaceOrderCommandHandler(
    ILogger<PlaceOrderCommandHandler> logger,
    IMapper mapper,
    IMediator mediator,
    IUserContext userContext,
    IInvoiceRepository invoiceRepository
) : IRequestHandler<PlaceOrderCommand, int>
{
    public async Task<int> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling PlaceOrderCommand for user: {UserId}", userContext.GetCurrentUser()?.Id);

        // Validate user context
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.User))
        {
            logger.LogWarning("Unauthorized attempt to place an order by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("You do not have permission to place an order.");
        }

        logger.LogInformation("Fetching cart items for user: {UserId} with promo code: {PromoCode}", currentUser.Id, request.PromoCode);

        // Fetch cart items
        var getCartQuery = new GetCartItemsQuery()
        {
            PromoCode = request.PromoCode
        };
        var cart = await mediator.Send(getCartQuery, cancellationToken);

        logger.LogInformation("Mapping cart to invoice for user: {UserId}", currentUser.Id);

        // Map cart to invoice
        var invoice = mapper.Map<Invoice>(cart);
        invoice.OrderStatus = OrderStatus.Pending;
        invoice.OrderDate = DateTime.UtcNow;
        invoice.UserId = currentUser.Id!;
        invoice.Notes = request.Notes??"";
        invoice.PromoCode = request.PromoCode??"";
        logger.LogInformation("Saving invoice to database for user: {UserId}", currentUser.Id);

        // Save invoice
        await invoiceRepository.AddAsync(invoice);

        logger.LogInformation("Clearing cart for user: {UserId}", currentUser.Id);

        // Clear cart
        var clearCartCommand = new ClearCartItemsCommand();
        await mediator.Send(clearCartCommand, cancellationToken);

        logger.LogInformation("Successfully placed order for user: {UserId}", currentUser.Id);

        return invoice.InvoiceId;
    }
}
