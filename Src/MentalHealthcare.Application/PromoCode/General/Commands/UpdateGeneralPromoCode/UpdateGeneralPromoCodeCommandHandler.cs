using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Commands.UpdateGeneralPromoCode;

public class UpdateGeneralPromoCodeCommandHandler(
    ILogger<UpdateGeneralPromoCodeCommandHandler> logger,
    IGeneralPromoCodeRepository generalPromoCodeRepository,
    IUserContext userContext
) : IRequestHandler<UpdateGeneralPromoCodeCommand>
{
    public async Task Handle(UpdateGeneralPromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateGeneralPromoCodeCommand for Promo Code ID: {PromoCodeId}",
            request.GeneralPromoCodeId);

        // Authorize user
        logger.LogInformation("Authorizing user for updating promo codes.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to update general promo codes.", currentUser.Id);

        // Fetch existing promo code
        logger.LogInformation("Fetching GeneralPromoCode with ID: {PromoCodeId}", request.GeneralPromoCodeId);
        var generalPromoCode =
            await generalPromoCodeRepository.GetGeneralPromoCodeByIdAsync(request.GeneralPromoCodeId);
        if (generalPromoCode == null)
        {
            logger.LogError("GeneralPromoCode with ID: {PromoCodeId} not found.", request.GeneralPromoCodeId);
            throw new KeyNotFoundException($"GeneralPromoCode with ID {request.GeneralPromoCodeId} not found.");
        }

        // Update Percentage
        if (request.Percentage.HasValue)
        {
            logger.LogInformation("Updating Percentage for GeneralPromoCode ID: {PromoCodeId} to {Percentage}",
                request.GeneralPromoCodeId, request.Percentage.Value);
            generalPromoCode.percentage = (float)Math.Round(request.Percentage.Value, 2);
        }

        // Update ExpireDate
        if (!string.IsNullOrEmpty(request.ExpireDate))
        {
            logger.LogInformation("Parsing ExpireDate: {ExpireDate} for GeneralPromoCode ID: {PromoCodeId}",
                request.ExpireDate, request.GeneralPromoCodeId);
            if (DateTime.TryParse(request.ExpireDate, out var parsedExpireDate))
            {
                logger.LogInformation("ExpireDate parsed successfully. Updating to {ParsedExpireDate}",
                    parsedExpireDate);
                if (parsedExpireDate <= DateTime.UtcNow)
                {
                    logger.LogWarning("ExpireDate {ParsedExpireDate} is not in the future. Rejecting update.",
                        parsedExpireDate);
                    throw new ArgumentException("ExpireDate must be a future date.");
                }

                generalPromoCode.expiredate = parsedExpireDate;
            }
            else
            {
                logger.LogError("Failed to parse ExpireDate: {ExpireDate} for GeneralPromoCode ID: {PromoCodeId}",
                    request.ExpireDate, request.GeneralPromoCodeId);
                throw new ArgumentException("Invalid ExpireDate format. Please provide a valid date.");
            }
        }

        // Update IsActive
        if (request.IsActive.HasValue)
        {
            logger.LogInformation("Updating IsActive for GeneralPromoCode ID: {PromoCodeId} to {IsActive}",
                request.GeneralPromoCodeId, request.IsActive.Value);
            generalPromoCode.isActive = request.IsActive.Value;
        }

        // Save Changes
        logger.LogInformation("Saving changes for GeneralPromoCode ID: {PromoCodeId}", request.GeneralPromoCodeId);
        await generalPromoCodeRepository.SaveChangesAsync();

        logger.LogInformation("GeneralPromoCode with ID: {PromoCodeId} updated successfully.",
            request.GeneralPromoCodeId);
    }
}