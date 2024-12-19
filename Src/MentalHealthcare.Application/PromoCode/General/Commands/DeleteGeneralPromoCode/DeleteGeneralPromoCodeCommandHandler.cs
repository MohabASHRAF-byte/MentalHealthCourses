using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Commands.DeleteGeneralPromoCode;

public class DeleteGeneralPromoCodeCommandHandler(
    ILogger<DeleteGeneralPromoCodeCommandHandler> logger,
    IGeneralPromoCodeRepository generalPromoCodeRepository
) : IRequestHandler<DeleteGeneralPromoCodeCommand>
{
    public async Task Handle(DeleteGeneralPromoCodeCommand request, CancellationToken cancellationToken)
    {
        //TODO: add auth and validation
        logger.LogInformation("Started deleting promo code with ID: {PromoCodeId}.", request.GeneralPromoCodeId);

        try
        {
            await generalPromoCodeRepository.DeleteGeneralPromoCodeByIdAsync(request.GeneralPromoCodeId);

            logger.LogInformation("Successfully deleted promo code with ID: {PromoCodeId}.", request.GeneralPromoCodeId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting promo code with ID: {PromoCodeId}.", request.GeneralPromoCodeId);
            throw; 
            //rethrow the exception to handle it higher in the stack
        }
    }
}