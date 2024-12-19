using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.DeleteCoursePromoCode;

public class DeleteCoursePromoCodeCommandHandler(
    ILogger<DeleteCoursePromoCodeCommandHandler> logger,
    IMapper mapper,
    ICoursePromoCodeRepository promoCodeRepository
    ):IRequestHandler<DeleteCoursePromoCodeCommand>
{
    public async Task Handle(DeleteCoursePromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteCoursePromoCodeCommandHandler");
       await promoCodeRepository.DeleteCoursePromoCodeByIdAsync(request.CoursePromoCodeId);
    }
}