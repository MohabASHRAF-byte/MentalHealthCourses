using MediatR;

namespace MentalHealthcare.Application.OrderProcessing.Order.Commands.Place;

public class PlaceOrderCommand : IRequest<int>
{
    public string? PromoCode { get; set; } = "";
    public string? Notes { get; set; } = "";
}