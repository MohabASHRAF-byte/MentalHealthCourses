using MediatR;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Delete_from_cart;

public class DeleteFromCartCommand:IRequest
{
    public int CourseId { get; set; }
}