using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;

public class ClearCartItemsCommand:IRequest<OperationResult<object>>
{
    
}