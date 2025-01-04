using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Commands.DeleteGeneralPromoCode;

public class DeleteGeneralPromoCodeCommandHandler(
    ILogger<DeleteGeneralPromoCodeCommandHandler> logger,
    IGeneralPromoCodeRepository generalPromoCodeRepository,
    IUserContext userContext
) : IRequestHandler<DeleteGeneralPromoCodeCommand>
{
    public async Task Handle(DeleteGeneralPromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to handle DeleteGeneralPromoCodeCommand for Promo Code ID: {PromoCodeId}", request.GeneralPromoCodeId);

        // Authorize user
        var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
        logger.LogInformation("User {UserId} authorized to delete general promo codes.", currentUser.Id);

        try
        {
            // Attempt to delete the promo code
            logger.LogInformation("Attempting to delete promo code with ID: {PromoCodeId}", request.GeneralPromoCodeId);
            await generalPromoCodeRepository.DeleteGeneralPromoCodeByIdAsync(request.GeneralPromoCodeId);

            logger.LogInformation("Successfully deleted promo code with ID: {PromoCodeId}", request.GeneralPromoCodeId);
        }
        catch (Exception ex)
        {
            // Log error and rethrow
            logger.LogError(ex, "Error occurred while deleting promo code with ID: {PromoCodeId}", request.GeneralPromoCodeId);
            throw;
        }
    }
}