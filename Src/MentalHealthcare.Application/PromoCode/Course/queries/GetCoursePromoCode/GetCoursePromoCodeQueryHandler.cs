using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.queries.GetCoursePromoCode;

public class GetCoursePromoCodeQueryHandler(
    ILogger<GetCoursePromoCodeQueryHandler> logger,
    IMapper mapper,
    ICoursePromoCodeRepository promoCodeRepository,
    IUserContext userContext
) : IRequestHandler<GetCoursePromoCodeQuery, CoursePromoCodeDto>
{
    public async Task<CoursePromoCodeDto> Handle(GetCoursePromoCodeQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetCoursePromoCodeQuery for Promo Code ID: {PromoCodeId}",
            request.CoursePromoCodeId);

        // Authorize user
        logger.LogInformation("Authorizing user for accessing promo code details.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to access promo code details.", currentUser.Id);

        // Fetch promo code details
        logger.LogInformation("Fetching promo code details for ID: {PromoCodeId}", request.CoursePromoCodeId);
        var coursePromoCode = await promoCodeRepository.GetCoursePromoCodeByIdAsync(request.CoursePromoCodeId);

        if (coursePromoCode == null)
        {
            logger.LogWarning("Promo code with ID: {PromoCodeId} not found.", request.CoursePromoCodeId);
            throw new KeyNotFoundException($"Promo code with ID {request.CoursePromoCodeId} not found.");
        }

        // Map to DTO
        var coursePromoCodeDto = mapper.Map<CoursePromoCodeDto>(coursePromoCode);
        coursePromoCodeDto.SecondsTillExpire =
            (long)coursePromoCodeDto.expiredate.Subtract(DateTime.UtcNow).TotalSeconds;
        coursePromoCodeDto.expiresInDays =
            (int)coursePromoCodeDto.expiredate.Subtract(DateTime.UtcNow).TotalDays;
        if (coursePromoCodeDto.SecondsTillExpire < 0)
        {
            coursePromoCodeDto.IsActive = false;
            coursePromoCodeDto.SecondsTillExpire = 0;
        }

        logger.LogInformation("Successfully fetched and mapped promo code details for ID: {PromoCodeId}",
            request.CoursePromoCodeId);

        return coursePromoCodeDto;
    }
}