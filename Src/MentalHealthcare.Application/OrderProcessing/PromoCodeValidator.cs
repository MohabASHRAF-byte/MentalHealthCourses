using MentalHealthcare.Domain.Dtos.OrderProcessing;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Application.OrderProcessing;

public static class PromoCodeValidator
{
    public static async Task<(decimal discountPercent, List<string> messages)> ValidatePromoCodeAsync(
        string? promoCode,
        IEnumerable<CourseCartDto> cartItems,
        IGeneralPromoCodeRepository generalPromoCodeRepository,
        ICoursePromoCodeRepository coursePromoCodeRepository,
        ILogger logger)
    {
        var discountPercent = 0m;
        var messages = new List<string>();

        // Materialize cartItems to prevent multiple enumeration
        var cartItemList = cartItems.ToList();

        logger.LogInformation("Validating promo code: {PromoCode}", promoCode);

        if (promoCode.IsNullOrEmpty())
        {
            logger.LogWarning("Promo code is null or empty.");
            messages.Add("No promo code provided.");
            return (discountPercent, messages);
        }

        // Fetch general promo code
        var generalPromoCode = await generalPromoCodeRepository
            .GetGeneralPromoCodeByPromoCodeNameAsync(promoCode!);
        if (generalPromoCode != null)
        {
            logger.LogInformation("General promo code found: {PromoCode}, Expiry: {ExpireDate}", generalPromoCode.Code,
                generalPromoCode.expiredate);

            if (generalPromoCode.expiredate >= DateTime.Now)
            {
                discountPercent = (decimal)generalPromoCode.percentage;
                logger.LogInformation("General promo code applied. Discount: {DiscountPercent}%", discountPercent);
            }
            else
            {
                messages.Add("Promo code expired.");
                logger.LogWarning("General promo code expired: {PromoCode}", promoCode);
            }

            // Log the result
            logger.LogInformation("Promo code validation completed. Discount: {DiscountPercent}%, Messages: {Messages}",
                discountPercent, string.Join(", ", messages));

            return (discountPercent, messages);
        }

        logger.LogWarning("General promo code not found: {PromoCode}", promoCode);

        // Validation: Multiple cart items with non-general promo code
        if (cartItemList.Count > 1)
        {
            messages.Add("Use a general promo code for more than one item. Remove all non-targeted courses.");
            logger.LogWarning("Promo code {PromoCode} cannot be applied to multiple items.", promoCode);
            return (discountPercent, messages);
        }

        // Check promo code for a single course
        if (cartItemList.Count == 1)
        {
            var courseId = cartItemList.First().CourseId;
            var coursePromoCode =
                await coursePromoCodeRepository.CheckIfPromoCodeAppliedForCourseAsync(promoCode!, courseId);
            if (coursePromoCode != null)
            {
                logger.LogInformation("Course promo code found: {PromoCode}, Expiry: {ExpireDate}",
                    coursePromoCode.Code, coursePromoCode.expiredate);

                if (coursePromoCode.expiredate >= DateTime.Now)
                {
                    discountPercent = (decimal)coursePromoCode.percentage;
                    logger.LogInformation("Course promo code applied. Discount: {DiscountPercent}%", discountPercent);
                }
                else
                {
                    messages.Add("Promo code expired.");
                    logger.LogWarning("Course promo code expired: {PromoCode}", promoCode);
                }
            }
            else
            {
                messages.Add("Promo code is invalid for this course.");
                logger.LogWarning("Promo code {PromoCode} is not valid for course ID {CourseId}.", promoCode, courseId);
            }
        }

        // Log the result
        logger.LogInformation("Promo code validation completed. Discount: {DiscountPercent}%, Messages: {Messages}",
            discountPercent, string.Join(", ", messages));

        return (discountPercent, messages);
    }
}