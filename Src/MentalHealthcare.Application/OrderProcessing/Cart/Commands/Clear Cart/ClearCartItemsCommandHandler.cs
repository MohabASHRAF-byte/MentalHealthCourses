using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;

public class ClearCartItemsCommandHandler(
    ILogger<ClearCartItemsCommandHandler> logger,
    ICartRepository cartRepository,
    IUserContext userContext
) : IRequestHandler<ClearCartItemsCommand , OperationResult<object>>
{
    public async Task<OperationResult<object>> Handle(ClearCartItemsCommand request, CancellationToken cancellationToken)
    {
        // Validate user context
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);
        await cartRepository.RemoveCartAsync(currentUser.Id);
        return OperationResult<object>.SuccessResult(new
        {
            Message = "Cart cleared successfully"
        });
    }
}