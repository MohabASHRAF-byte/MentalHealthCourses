using AutoMapper;
using MediatR;
using MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;
using MentalHealthcare.Domain.Dtos.PromoCode;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.queries.GetCoursePromoCode;

public class GetCoursePromoCodeQueryHandler(
    ILogger<AddCoursePromoCodeCommandHandler> logger,
    IMapper mapper,
    ICoursePromoCodeRepository promoCodeRepository
) : IRequestHandler<GetCoursePromoCodeQuery, CoursePromoCodeDto>
{
    public async Task<CoursePromoCodeDto> Handle(GetCoursePromoCodeQuery request, CancellationToken cancellationToken)
    {
        //TODO: add auth 
        logger.LogInformation("Handling GetCoursePromoCodeQuery");
        var coursePromoCode = await promoCodeRepository
            .GetCoursePromoCodeByIdAsync(request.CoursePromoCodeId);
        var coursePromoCodeDto = mapper.Map<CoursePromoCodeDto>(coursePromoCode);
        return coursePromoCodeDto;
    }
}