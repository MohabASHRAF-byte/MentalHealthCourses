using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Update;

public class UpdatePendingAdminCommandHandler(
    ILogger<UpdatePendingAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<UpdatePendingAdminCommand>
{
    public async Task Handle(UpdatePendingAdminCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving the current user from the context.");
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);


        logger.LogInformation("Fetching admin by identity for user ID: {Id}", currentUser.Id);
        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

        if (admin == null)
        {
            logger.LogWarning("Admin with identity {Id} does not exist.", currentUser.Id);
            throw new ForBidenException(
                localizationService.GetMessage("AdminIdentityNotExist", "Admin with given identity does not exist.")
            );
        }

        var adminId = admin.AdminId;
        logger.LogInformation("Attempting to update pending admin email from {OldEmail} to {NewEmail}",
            request.OldEmail, request.NewEmail);
        var result = await adminRepository.UpdatePendingAsync(request.OldEmail, request.NewEmail, adminId);

        if (!result)
        {
            logger.LogWarning("Old email {OldEmail} not found.", request.OldEmail);
            throw new ResourceNotFound(
                localizationService.GetMessage("OldEmailNotFound", "Old email not found: {0}", request.OldEmail),
                request.OldEmail
            );
        }

        logger.LogInformation("Successfully updated pending admin email for Admin ID: {AdminId}", adminId);
    }
}