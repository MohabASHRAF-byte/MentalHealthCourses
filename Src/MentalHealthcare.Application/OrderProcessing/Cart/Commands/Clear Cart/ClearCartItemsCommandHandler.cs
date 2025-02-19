using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.OrderProcessing;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;

public class ClearCartItemsCommandHandler(
    ILogger<ClearCartItemsCommandHandler> logger,
    ICartRepository cartRepository,
    IUserContext userContext
) : IRequestHandler<ClearCartItemsCommand>
{
    public async Task Handle(ClearCartItemsCommand request, CancellationToken cancellationToken)
    {
        // Validate user context
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.User], logger);
        await cartRepository.RemoveCartAsync(currentUser.Id);
    }
}