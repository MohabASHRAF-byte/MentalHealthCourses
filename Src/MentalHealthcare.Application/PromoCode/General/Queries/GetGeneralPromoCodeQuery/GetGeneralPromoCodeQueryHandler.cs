using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Queries.GetGeneralPromoCodeQuery;

public class GetGeneralPromoCodeQueryHandler(
    ILogger<GetGeneralPromoCodeQueryHandler> logger,
    IMapper mapper,
    IGeneralPromoCodeRepository generalPromoCodeRepository,
    IUserContext userContext
) : IRequestHandler<GetGeneralPromoCodeQuery, GeneralPromoCodeDto>
{
    public async Task<GeneralPromoCodeDto> Handle(GetGeneralPromoCodeQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetGeneralPromoCodeQuery for Promo Code ID: {PromoCodeId}", request.PromoCodeId);

        // Authorize user
        logger.LogInformation("Authorizing user for accessing general promo codes.");
        var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
        logger.LogInformation("User {UserId} authorized to access general promo codes.", currentUser.Id);

        // Fetch promo code details
        logger.LogInformation("Fetching general promo code details for ID: {PromoCodeId}", request.PromoCodeId);
        var generalPromoCode = await generalPromoCodeRepository.GetGeneralPromoCodeByIdAsync(request.PromoCodeId);

        if (generalPromoCode == null)
        {
            logger.LogWarning("General promo code with ID: {PromoCodeId} not found.", request.PromoCodeId);
            throw new KeyNotFoundException($"General promo code with ID {request.PromoCodeId} not found.");
        }

        // Map to DTO
        var generalPromoCodeDto = mapper.Map<GeneralPromoCodeDto>(generalPromoCode);
        generalPromoCodeDto.expiresInSeconds =
            (int)generalPromoCodeDto.expiredate.Subtract(DateTime.UtcNow).TotalSeconds;
        if (generalPromoCodeDto.expiresInSeconds < 0)
        {
            generalPromoCodeDto.isActive = false;
            generalPromoCodeDto.expiresInSeconds = 0;
        }
        logger.LogInformation("Successfully fetched and mapped general promo code details for ID: {PromoCodeId}", request.PromoCodeId);

        return generalPromoCodeDto;
    }
}
