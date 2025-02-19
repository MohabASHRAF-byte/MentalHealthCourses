using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Commands.AddGeneralPromoCode;

public class AddGeneralPromoCodeCommandHandler(
    ILogger<AddGeneralPromoCodeCommandHandler> logger,
    IMapper mapper,
    IGeneralPromoCodeRepository generalPromoCodeRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<AddGeneralPromoCodeCommand, int>
{
    public async Task<int> Handle(AddGeneralPromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {CommandName} for Promo Code: {Code}", nameof(AddGeneralPromoCodeCommand),
            request.Code);

        // Authorize user
        var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
        logger.LogInformation("User {UserId} authorized to add general promo codes.", currentUser.Id);

        try
        {
            logger.LogInformation("Received AddGeneralPromoCodeCommand: {@Request}", request);

            // Validate and parse ExpireDate
            if (!DateTime.TryParse(request.ExpireDate, out var parsedExpireDate))
            {
                logger.LogWarning("Invalid ExpireDate format received: {ExpireDate}", request.ExpireDate);
                throw new BadHttpRequestException(
                    localizationService.GetMessage("InvalidExpireDateFormat")
                );
            }

            // Map the request to the entity
            var generalPromoCode = mapper.Map<GeneralPromoCode>(request);
            generalPromoCode.expiredate = parsedExpireDate;

            logger.LogInformation("Mapped AddGeneralPromoCodeCommand to GeneralPromoCode: {@GeneralPromoCode}",
                generalPromoCode);

            // Add to repository
            await generalPromoCodeRepository.AddGeneralPromoCodeAsync(generalPromoCode);

            logger.LogInformation("Successfully added GeneralPromoCode with Code: {Code} and ID: {PromoCodeId}",
                generalPromoCode.Code, generalPromoCode.GeneralPromoCodeId);

            return generalPromoCode.GeneralPromoCodeId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while handling {CommandName} for Promo Code: {Code}",
                nameof(AddGeneralPromoCodeCommand), request.Code);
            throw;
        }
    }
}