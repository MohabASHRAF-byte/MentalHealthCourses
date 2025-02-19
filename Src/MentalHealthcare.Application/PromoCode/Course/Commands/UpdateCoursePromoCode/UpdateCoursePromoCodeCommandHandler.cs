using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static System.DateTime;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.UpdateCoursePromoCode;

public class UpdateCoursePromoCodeCommandHandler(
    ILogger<UpdateCoursePromoCodeCommandHandler> logger,
    ICoursePromoCodeRepository promoCodeRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<UpdateCoursePromoCodeCommand, int>
{
    public async Task<int> Handle(UpdateCoursePromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateCoursePromoCodeCommand for Promo Code ID: {PromoCodeId}",
            request.CoursePromoCodeId);

        // Authorization
        logger.LogInformation("Authorizing user for updating promo codes.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to update promo codes.", currentUser.Id);

        // Fetch existing promo code
        logger.LogInformation("Fetching CoursePromoCode with ID: {PromoCodeId}", request.CoursePromoCodeId);
        var coursePromoCode = await promoCodeRepository.GetCoursePromoCodeByIdAsync(request.CoursePromoCodeId);
        if (coursePromoCode == null)
        {
            logger.LogError("CoursePromoCode with ID: {PromoCodeId} not found.", request.CoursePromoCodeId);
            throw new ResourceNotFound(
                "CoursePromoCode", 
                "كود خصم الدورة", 
                request.CoursePromoCodeId.ToString());
        }

        // Update Percentage
        if (request.Percentage.HasValue)
        {
            logger.LogInformation("Updating Percentage for CoursePromoCode ID: {PromoCodeId} to {Percentage}",
                request.CoursePromoCodeId, request.Percentage.Value);
            coursePromoCode.percentage = (float)Math.Round(request.Percentage.Value, 2);
        }

        // Update ExpireDate
        if (!string.IsNullOrEmpty(request.ExpireDate))
        {
            logger.LogInformation("Parsing ExpireDate: {ExpireDate} for CoursePromoCode ID: {PromoCodeId}",
                request.ExpireDate, request.CoursePromoCodeId);
            if (TryParse(request.ExpireDate, out var parsedExpireDate))
            {
                logger.LogInformation("ExpireDate parsed successfully. Updating to {ParsedExpireDate}",
                    parsedExpireDate);
                if (parsedExpireDate <= UtcNow)
                {
                    logger.LogWarning("ExpireDate {ParsedExpireDate} is not in the future. Rejecting update.",
                        parsedExpireDate);
                    throw new BadHttpRequestException(
                        localizationService.GetMessage("ExpireDateMustBeFuture")
                    );
                }

                coursePromoCode.expiredate = parsedExpireDate;
            }
            else
            {
                logger.LogError("Failed to parse ExpireDate: {ExpireDate} for CoursePromoCode ID: {PromoCodeId}",
                    request.ExpireDate, request.CoursePromoCodeId);
                throw new BadHttpRequestException(
                    localizationService.GetMessage("InvalidExpireDateFormat")
                );
            }
        }

        if (request.IsActive.HasValue)
            coursePromoCode.IsActive = request.IsActive.Value;
        // Save Changes
        logger.LogInformation("Saving changes for CoursePromoCode ID: {PromoCodeId}", request.CoursePromoCodeId);
        await promoCodeRepository.SaveChangesAsync();

        logger.LogInformation("CoursePromoCode with ID: {PromoCodeId} updated successfully.",
            request.CoursePromoCodeId);
        return coursePromoCode.CoursePromoCodeId;
    }
}