using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;
using static System.DateTime;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.UpdateCoursePromoCode;

public class UpdateCoursePromoCodeCommandHandler(
    ILogger<UpdateCoursePromoCodeCommandHandler> logger,
    ICoursePromoCodeRepository promoCodeRepository
) : IRequestHandler<UpdateCoursePromoCodeCommand, int>
{
    public async Task<int> Handle(UpdateCoursePromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateCoursePromoCodeCommandHandler invoked.");

        // Log for fetching the promo code
        logger.LogInformation($"Fetching CoursePromoCode with ID: {request.CoursePromoCodeId}");
        //TODO:  ADD AUTH AND VAL 
        var coursePromoCode = await promoCodeRepository.GetCoursePromoCodeByIdAsync(request.CoursePromoCodeId);

        // Log for percentage update
        if (request.Percentage.HasValue)
        {
            logger.LogInformation($"Updating Percentage for CoursePromoCode ID: {request.CoursePromoCodeId} to {request.Percentage.Value}");
            coursePromoCode.percentage = (float)Math.Round(request.Percentage.Value, 2);
        }

        // Log for expire date update
        if (request.ExpireDate != null)
        {
            logger.LogInformation($"Attempting to parse ExpireDate: {request.ExpireDate} for CoursePromoCode ID: {request.CoursePromoCodeId}");
            var tryParse = TryParse(request.ExpireDate, out var parsedExpireDate);
            if (tryParse)
            {
                logger.LogInformation($"ExpireDate parsed successfully. Updating to {parsedExpireDate}");
                coursePromoCode.expiredate = parsedExpireDate;
            }
            else
            {
                logger.LogWarning($"Failed to parse ExpireDate: {request.ExpireDate} for CoursePromoCode ID: {request.CoursePromoCodeId}");
            }
        }

        logger.LogInformation($"Saving changes for CoursePromoCode ID: {request.CoursePromoCodeId}");
        await promoCodeRepository.saveChangesAsync();

        logger.LogInformation($"CoursePromoCode with ID: {request.CoursePromoCodeId} updated successfully.");
        return coursePromoCode.CoursePromoCodeId;
    }
}
