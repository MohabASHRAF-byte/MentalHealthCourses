using System.Diagnostics.CodeAnalysis;
using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Add;

/// <summary>
/// Handles the addition of a new pending admin user.
/// </summary>
public class AddAdminCommandHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext,
    ILocalizationService localizationService
) : IRequestHandler<AddAdminCommand>
{
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 27605")]
    public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling AddAdminCommand for Email: {Email}", request.Email);

        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin ], logger);
        logger.LogInformation("Admin user {AdminId} is attempting to add a new admin with Email: {Email}",
            currentUser.Id, request.Email);
    
        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);
        
        if (await adminRepository.IsPendingExistAsync(request.Email))
        {
            logger.LogWarning("Pending admin user already exists with Email: {Email}", request.Email);
            throw new AlreadyExist(
                string.Format(
                    localizationService.GetMessage("EmailAlreadyPending", "The email {0} is already pending."),
                    request.Email
                )
            );
        }

        if (await adminRepository.IsExistAsync(request.Email))
        {
            logger.LogWarning("Admin user already exists with Email: {Email}", request.Email);
            throw new AlreadyExist(
                string.Format(
                    localizationService.GetMessage("EmailAlreadyAdmin", "The email {0} is already an admin."),
                    request.Email
                )
            );
        }

        await adminRepository.AddPendingAsync(request.Email, admin.AdminId);
        logger.LogInformation("Successfully added a new pending admin with Email: {Email} by Admin: {AdminId}",
            request.Email, currentUser.Id);
    }
}
