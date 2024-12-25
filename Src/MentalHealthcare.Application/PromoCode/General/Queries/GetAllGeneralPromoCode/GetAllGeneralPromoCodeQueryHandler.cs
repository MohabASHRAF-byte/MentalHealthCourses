using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.General.Queries.GetAllGeneralPromoCode;

public class GetAllGeneralPromoCodeQueryHandler(
    ILogger<GetAllGeneralPromoCodeQueryHandler> logger,
    IGeneralPromoCodeRepository generalPromoCodeRepository
) : IRequestHandler<GetAllGeneralPromoCodeQuery, PageResult<GeneralPromoCodeDto>>
{
    public async Task<PageResult<GeneralPromoCodeDto>> Handle(GetAllGeneralPromoCodeQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handle Initiated for GetAllGeneralPromoCodeQueryHandler with PageNumber: {PageNumber}, PageSize: {PageSize}, SearchText: {SearchText}, IsActive: {IsActive}",
            request.PageNumber, request.PageSize, request.SearchText, request.IsActive);

        logger.LogInformation("Fetching promo codes");
        var promoCodes = await generalPromoCodeRepository
            .GetGeneralPromoCodeAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchText,
                request.IsActive
            );

        logger.LogInformation("Promo codes fetched successfully . Total records: {TotalRecords}",
            promoCodes.Item2);

        logger.LogInformation("Returning PageResult with {TotalPages} total pages and {TotalRecords} total records",
            promoCodes.Item1, promoCodes.Item2);

        return new PageResult<GeneralPromoCodeDto>(
            promoCodes.Item2,
            promoCodes.Item1,
            request.PageSize,
            request.PageNumber
        );
    }
}