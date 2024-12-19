using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Commands.UpdateGeneralPromoCode;

public class UpdateGeneralPromoCodeCommandHandler(
    ILogger<UpdateGeneralPromoCodeCommandHandler> logger,
    IGeneralPromoCodeRepository generalPromoCodeRepository
    ):IRequestHandler<UpdateGeneralPromoCodeCommand>
{
    public async Task Handle(UpdateGeneralPromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateGeneralPromoCodeCommandHandler invoked.");

        // Log for fetching the promo code
        logger.LogInformation($"Fetching generalPromoCode with ID: {request.GeneralPromoCodeId}");
        //TODO:  ADD AUTH AND VAL 
        var generalPromoCode = await generalPromoCodeRepository.GetGeneralPromoCodeByIdAsync(request.GeneralPromoCodeId);

        // Log for percentage update
        if (request.Percentage.HasValue)
        {
            logger.LogInformation($"Updating Percentage for generalPromoCode ID: {request.GeneralPromoCodeId} to {request.Percentage.Value}");
            generalPromoCode.percentage = (float)Math.Round(request.Percentage.Value, 2);
        }

        // Log for expire date update
        if (request.ExpireDate != null)
        {
            logger.LogInformation($"Attempting to parse ExpireDate: {request.ExpireDate} for generalPromoCode ID: {request.GeneralPromoCodeId}");
            var tryParse = DateTime.TryParse(request.ExpireDate, out var parsedExpireDate);
            if (tryParse)
            {
                logger.LogInformation($"ExpireDate parsed successfully. Updating to {parsedExpireDate}");
                generalPromoCode.expiredate = parsedExpireDate;
            }
            else
            {
                logger.LogWarning($"Failed to parse ExpireDate: {request.ExpireDate} for generalPromoCode ID: {request.GeneralPromoCodeId}");
            }
        }

        logger.LogInformation($"Saving changes for generalPromoCode ID: {request.GeneralPromoCodeId}");
        await generalPromoCodeRepository.SaveChangesAsync();

        logger.LogInformation($"generalPromoCode with ID: {request.GeneralPromoCodeId} updated successfully.");
    }
}