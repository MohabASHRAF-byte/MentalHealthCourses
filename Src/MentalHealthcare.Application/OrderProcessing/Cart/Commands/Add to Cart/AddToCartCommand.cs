using MediatR;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Add_to_Cart;

public class AddToCartCommand:IRequest<int>
{
    public int CourseId { get; set; }
}