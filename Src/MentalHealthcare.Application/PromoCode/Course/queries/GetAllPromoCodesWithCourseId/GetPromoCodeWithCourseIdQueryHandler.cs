using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.queries.GetAllPromoCodesWithCourseId;

public class GetPromoCodeWithCourseIdQueryHandler(
    ILogger<GetPromoCodeWithCourseIdQueryHandler> logger,
    ICoursePromoCodeRepository promoCodeRepository,
    IUserContext userContext
) : IRequestHandler<GetPromoCodeWithCourseIdQuery, PageResult<CoursePromoCodeDto>>
{
    public async Task<PageResult<CoursePromoCodeDto>> Handle(GetPromoCodeWithCourseIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Initiating GetPromoCodeWithCourseIdQuery for CourseId: {CourseId}, PageNumber: {PageNumber}, PageSize: {PageSize}, SearchText: {SearchText}, IsActive: {IsActive}",
            request.CourseId, request.PageNumber, request.PageSize, request.SearchText, request.IsActive);

        // Authorize user
        logger.LogInformation("Authorizing user for accessing promo codes.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to access promo codes.", currentUser.Id);

        // Fetch promo codes
        logger.LogInformation("Fetching promo codes for CourseId: {CourseId}", request.CourseId);
        var promoCodes = await promoCodeRepository
            .GetCoursePromoCodeByCourseIdAsync(
                request.CourseId,
                request.PageNumber,
                request.PageSize,
                request.SearchText,
                request.IsActive
            );
        foreach (var promoCode in promoCodes.Item2)
        {
            promoCode.SecondsTillExpire =
                (long)promoCode.expiredate.Subtract(DateTime.UtcNow).TotalSeconds;
            if (promoCode.SecondsTillExpire < 0)
            {
                promoCode.IsActive = false;
                promoCode.SecondsTillExpire = 0;
            }
        }

        logger.LogInformation(
            "Promo codes fetched successfully for CourseId: {CourseId}. Total records: {TotalRecords}",
            request.CourseId, promoCodes.Item1);

        // Construct and return result
        var result = new PageResult<CoursePromoCodeDto>(
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