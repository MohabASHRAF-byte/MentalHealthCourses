using AutoMapper;
using MediatR;
using MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Place;

public class PlaceOrderCommandHandler(
    ILogger<PlaceOrderCommandHandler> logger,
    IMapper mapper,
    IMediator mediator
    ): IRequestHandler<PlaceOrderCommand,int>
{
    public async Task<int> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        
        var getCartQuery = new GetCartItemsQuery()
        {
            PromoCode = request.PromoCode
        };
        var cart = await mediator.Send(getCartQuery, cancellationToken);
        var invoice = mapper.Map<Invoice>(cart);
        invoice.OrderStatus = OrderStatus.Pending;
        invoice.OrderDate = DateTime.UtcNow;

        return 0;
    }
}