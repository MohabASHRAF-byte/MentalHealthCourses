using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Commands.AddGeneralPromoCode;

public class AddGeneralPromoCodeCommandHandler(
    ILogger<AddGeneralPromoCodeCommandHandler> logger,
    IMapper mapper,
    IGeneralPromoCodeRepository generalPromoCodeRepository
) : IRequestHandler<AddGeneralPromoCodeCommand, int>
{
    public async Task<int> Handle(AddGeneralPromoCodeCommand request, CancellationToken cancellationToken)
    {
        //TODO: add auth and validation
        logger.LogInformation("Starting to handle {CommandName}", nameof(AddGeneralPromoCodeCommand));

        try
        {
            logger.LogInformation("Received AddGeneralPromoCodeCommand: {@Request}", request);

            // Map the request to the entity
            var generalPromoCode = mapper.Map<GeneralPromoCode>(request);

            // Log mapping result
            logger.LogInformation("Mapped AddGeneralPromoCodeCommand to generalPromoCode: {@generalPromoCode}",
                generalPromoCode.Code);

            if (!DateTime.TryParse(request.ExpireDate, out var parsedExpireDate))
            {
                logger.LogWarning("Invalid ExpireDate format received: {ExpireDate}", request.ExpireDate);
                throw new ArgumentException("Invalid date format for ExpireDate.");
            }

            generalPromoCode.expiredate = parsedExpireDate;
    
            // Add to repository
            await generalPromoCodeRepository.AddGeneralPromoCodeAsync(generalPromoCode);

            logger.LogInformation("Successfully added generalPromoCode with Code: {Code} ",
                request.Code);
            return generalPromoCode.GeneralPromoCodeId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while handling {CommandName} for Code: {Code}",
                nameof(AddGeneralPromoCodeCommand), request.Code);
            throw;
        }
    }
}