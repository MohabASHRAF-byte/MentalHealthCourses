using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.queries.GetAllPromoCodesWithCourseId;

public class GetPromoCodeWithCourseIdQueryHandler(
    ILogger<GetPromoCodeWithCourseIdQueryHandler> logger,
    ICoursePromoCodeRepository promoCodeRepository
) : IRequestHandler<GetPromoCodeWithCourseIdQuery, PageResult<CoursePromoCodeDto>>

{
    public async Task<PageResult<CoursePromoCodeDto>> Handle(GetPromoCodeWithCourseIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handle Initiated for GetPromoCodeWithCourseIdQuery with CourseId: {CourseId}, PageNumber: {PageNumber}, PageSize: {PageSize}, SearchText: {SearchText}, IsActive: {IsActive}",
            request.CourseId, request.PageNumber, request.PageSize, request.SearchText, request.IsActive);

        logger.LogInformation("Fetching promo codes for CourseId: {CourseId}", request.CourseId);
        var promoCodes = await promoCodeRepository
            .GetCoursePromoCodeByCourseIdAsync(
                request.CourseId,
                request.PageNumber,
                request.PageSize,
                request.SearchText,
                request.IsActive
            );

        logger.LogInformation("Promo codes fetched successfully for CourseId: {CourseId}. Total records: {TotalRecords}", 
            request.CourseId, promoCodes.Item2);

        logger.LogInformation("Returning PageResult with {TotalPages} total pages and {TotalRecords} total records",
            promoCodes.Item1, promoCodes.Item2);

        return new PageResult<CoursePromoCodeDto>(
            promoCodes.Item2,
            promoCodes.Item1,
            request.PageSize,
            request.PageNumber
        );
    }
}