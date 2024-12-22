using MediatR;
using MentalHealthcare.Domain.Dtos.OrderProcessing;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;

public class GetCartItemsQuery:IRequest<IEnumerable<CourseCartDto>>
{
    
}