using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Update;

/// <summary>
/// Handles updating a pending admin's email.
/// </summary>
/// <param name="request">The update request containing old and new emails.</param>
/// <param name="cancellationToken">Token to handle cancellation of the request.</param>
/// <returns>No thing</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the retrieval of the current user from the context.</description>
/// </item>
/// <item>
/// <description>Check if the current user is authorized: 
/// <list type="bullet">
/// <item>Fetch the current user from <c>userContext</c>.</item>
/// <item>Verify if the user has the Admin role. If not, log a warning and throw an exception.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Fetch the admin by identity:
/// <list type="bullet">
/// <item>Retrieve the admin using <c>GetAdminByIdentityAsync</c>.</item>
/// <item>If the admin does not exist, log a warning and throw an exception.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Update the pending admin's email:
/// <list type="bullet">
/// <item>Log the attempt to update the email from old to new.</item>
/// <item>Call <c>UpdatePendingAsync</c> to perform the email update.</item>
/// <item>If the update fails, log a warning and throw an exception indicating the old email was not found.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Log success: 
/// <list type="bullet">
/// <item>Log a success message indicating the update was successful.</item>
/// </list>
/// </description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ForBidenException">
/// Thrown if the current user is not an admin or if the admin identity does not exist.
/// </exception>
/// <exception cref="ResourceNotFound">Thrown if the old email does not exist.</exception>
public class UpdatePendingAdminCommandHandler(
    ILogger<UpdatePendingAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext
) : IRequestHandler<UpdatePendingAdminCommand>
{
    public async Task Handle(UpdatePendingAdminCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving the current user from the context.");
        var currentUser = userContext.GetCurrentUser();
        
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning("Permission denied for non-admin user to update pending admin.");
            throw new ForBidenException("Don't have the permission to add admin users.");
        }

        logger.LogInformation("Fetching admin by identity for user ID: {Id}", currentUser.Id);
        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);
        
        if (admin == null)
        {
            logger.LogWarning("Admin with identity {Id} does not exist.", currentUser.Id);
            throw new ForBidenException("Admin with given identity does not exist.");
        }
        
        var adminId = admin.AdminId;
        logger.LogInformation("Attempting to update pending admin email from {OldEmail} to {NewEmail}", request.OldEmail, request.NewEmail);
        var result = await adminRepository.UpdatePendingAsync(request.OldEmail, request.NewEmail, adminId);
        
        if (!result)
        {
            logger.LogWarning("Old email {OldEmail} not found.", request.OldEmail);
            throw new ResourceNotFound("Old Mail", request.OldEmail);
        }

        logger.LogInformation("Successfully updated pending admin email for Admin ID: {AdminId}", adminId);
    }
}
