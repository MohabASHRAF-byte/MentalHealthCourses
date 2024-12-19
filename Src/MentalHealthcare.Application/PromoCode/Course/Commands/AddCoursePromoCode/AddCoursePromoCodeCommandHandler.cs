using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;

public class AddCoursePromoCodeCommandHandler(
    ILogger<AddCoursePromoCodeCommandHandler> logger,
    IMapper mapper,
    ICoursePromoCodeRepository promoCodeRepository
) : IRequestHandler<AddCoursePromoCodeCommand,int>
{
    public async Task<int> Handle(AddCoursePromoCodeCommand request, CancellationToken cancellationToken)
    {
        //TODO: add auth and validation
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
                throw new ArgumentException("Invalid date format for ExpireDate.");
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