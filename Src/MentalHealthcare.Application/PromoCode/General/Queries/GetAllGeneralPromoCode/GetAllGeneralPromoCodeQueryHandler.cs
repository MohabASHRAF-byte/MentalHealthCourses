using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Queries.GetAllGeneralPromoCode;

public class GetAllGeneralPromoCodeQueryHandler(
    ILogger<GetAllGeneralPromoCodeQueryHandler> logger,
    IGeneralPromoCodeRepository generalPromoCodeRepository,
    IUserContext userContext
) : IRequestHandler<GetAllGeneralPromoCodeQuery, PageResult<GeneralPromoCodeDto>>
{
    public async Task<PageResult<GeneralPromoCodeDto>> Handle(GetAllGeneralPromoCodeQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handle initiated for GetAllGeneralPromoCodeQueryHandler with PageNumber: {PageNumber}, PageSize: {PageSize}, SearchText: {SearchText}, IsActive: {IsActive}",
            request.PageNumber, request.PageSize, request.SearchText, request.IsActive);

        // Authorize user
        logger.LogInformation("Authorizing user for accessing general promo codes.");
        var currentUser = userContext.EnsureAuthorizedUser(new() { UserRoles.Admin }, logger);
        logger.LogInformation("User {UserId} authorized to access general promo codes.", currentUser.Id);

        // Fetch promo codes
        logger.LogInformation("Fetching general promo codes.");
        var promoCodes = await generalPromoCodeRepository
            .GetGeneralPromoCodeAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchText,
                request.IsActive
            );

        logger.LogInformation("General promo codes fetched successfully. Total records: {TotalRecords}",
            promoCodes.Item1);
        foreach (var promoCode in promoCodes.Item2)
        {
            promoCode.expiresInSeconds =
                (int)promoCode.expiredate.Subtract(DateTime.UtcNow).TotalSeconds;
            if (promoCode.expiresInSeconds < 0)
            {
                promoCode.isActive = false;
                promoCode.expiresInSeconds = 0;
            }
        }

        // Returning the result
        var result = new PageResult<GeneralPromoCodeDto>(
            promoCodes.Item2,
            promoCodes.Item1,
            request.PageSize,
            request.PageNumber
        );

        logger.LogInformation("Returning PageResult with {TotalPages} total pages and {TotalRecords} total records",
            result.TotalPages, promoCodes.TotalCount);

        return result;
    }
}