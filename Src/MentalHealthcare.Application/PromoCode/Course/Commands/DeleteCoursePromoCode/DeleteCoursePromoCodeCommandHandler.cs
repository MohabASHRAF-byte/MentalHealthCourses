using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories.PromoCode;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.PromoCode.Course.Commands.DeleteCoursePromoCode;

public class DeleteCoursePromoCodeCommandHandler(
    ILogger<DeleteCoursePromoCodeCommandHandler> logger,
    ICoursePromoCodeRepository promoCodeRepository,
    IUserContext userContext
) : IRequestHandler<DeleteCoursePromoCodeCommand>
{
    public async Task Handle(DeleteCoursePromoCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting DeleteCoursePromoCodeCommandHandler for Promo Code ID: {PromoCodeId}",
            request.CoursePromoCodeId);

        // Authenticate and authorize the user
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to delete promo codes.", currentUser.Id);

        // Attempt to delete the promo code
        logger.LogInformation("Deleting Promo Code ID: {PromoCodeId}", request.CoursePromoCodeId);
        await promoCodeRepository.DeleteCoursePromoCodeByIdAsync(request.CoursePromoCodeId);

        logger.LogInformation("Successfully deleted Promo Code ID: {PromoCodeId}", request.CoursePromoCodeId);
    }
}