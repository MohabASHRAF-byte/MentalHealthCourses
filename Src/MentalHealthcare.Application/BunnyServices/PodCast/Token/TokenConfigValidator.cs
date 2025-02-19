using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.BunnyServices.PodCast.Token
{
    internal static class TokenConfigValidator
    {
        internal static void EnsureValid(TokenConfig config)
        {
            if (string.IsNullOrEmpty(config.SecurityKey))
                throw new BadHttpRequestException("يرجى تعيين مفتاح الأمان.");

            if (!config.HasExpiresAt)
                throw new BadHttpRequestException("يرجى تعيين تاريخ انتهاء الصلاحية.");

            if (config.CountriesBlocked.Intersect(config.CountriesAllowed).Any())
                throw new BadHttpRequestException("هناك دول في كل من قائمة الدول المسموح بها والمحظورة.");
        }
    }
}