using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;

public class AddCoursePromoCodeCommandHandler(
    ILogger<AddCoursePromoCodeCommandHandler> logger,
    IMapper mapper,
    ICoursePromoCodeRepository promoCodeRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<AddCoursePromoCodeCommand, int>
{
    public async Task<int> Handle(AddCoursePromoCodeCommand request, CancellationToken cancellationToken)
    {
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("Starting to handle {CommandName}", nameof(AddCoursePromoCodeCommand));

        try
        {
            logger.LogInformation("Received AddCoursePromoCodeCommand: {@Request}", request);

            // Map the request to the entity
            var coursePromoCode = mapper.Map<CoursePromoCode>(request);

            // Log mapping result
            logger.LogInformation("Mapped AddCoursePromoCodeCommand to CoursePromoCode: {@CoursePromoCode}",
                coursePromoCode);

            if (!DateTime.TryParse(request.ExpireDate, out var parsedExpireDate))
            {
                logger.LogWarning("Invalid ExpireDate format received: {ExpireDate}", request.ExpireDate);
                throw new BadHttpRequestException(
                    localizationService.GetMessage("InvalidExpireDateFormat")
                );
            }

            coursePromoCode.expiredate = parsedExpireDate;

            // Add to repository
            await promoCodeRepository.AddCoursePromoCodeAsync(coursePromoCode);

            logger.LogInformation("Successfully added CoursePromoCode with Code: {Code} for CourseId: {CourseId}",
                request.Code, request.CourseId);
            return coursePromoCode.CoursePromoCodeId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while handling {CommandName} for Code: {Code}, CourseId: {CourseId}",
                nameof(AddCoursePromoCodeCommand), request.Code, request.CourseId);
            throw;
        }
    }
}