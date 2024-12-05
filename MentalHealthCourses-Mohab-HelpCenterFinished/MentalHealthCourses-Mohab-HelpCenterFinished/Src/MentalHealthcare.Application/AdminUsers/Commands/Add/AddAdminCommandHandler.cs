using System.Diagnostics.CodeAnalysis;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Add;
/// <summary>
/// Handles the addition of a new pending admin user.
/// </summary>
/// <param name="request">The command containing the email to add as a pending admin.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>A task that represents the completion of the operation.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the start of command handling: Logs when the command is being handled and the email involved.</description>
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
/// <description>Check if the email is already pending: 
/// <list type="bullet">
/// <item>Call <c>IsPendingExistAsync</c> to verify if the email is already in the pending list.</item>
/// <item>If true, log a warning and throw an exception.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Check if the email is already an admin: 
/// <list type="bullet">
/// <item>Call <c>IsExistAsync</c> to see if the email already belongs to an existing admin.</item>
/// <item>If true, log a warning and throw an exception.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Add the pending admin: 
/// <list type="bullet">
/// <item>If all checks pass, add the new pending admin by calling <c>AddPendingAsync</c>.</item>
/// <item>Log a success message once the admin is added.</item>
/// </list>
/// </description>
/// </item>
/// </list>
/// </remarks>
public class AddAdminCommandHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IUserContext userContext
) : IRequestHandler<AddAdminCommand>
{
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 27605")]
    public async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling AddAdminCommand for Email: {Email}", request.Email);

        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        {
            logger.LogWarning("Unauthorized attempt to add admin by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission to add admin users.");
        }

        logger.LogInformation("Admin user {AdminId} is attempting to add a new admin with Email: {Email}",
            currentUser.Id, request.Email);

        var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

        if (await adminRepository.IsPendingExistAsync(request.Email))
        {
            logger.LogWarning("Pending admin user already exists with Email: {Email}", request.Email);
            throw new AlreadyExist($"Email {request.Email} already pending.");
        }

        if (await adminRepository.IsExistAsync(request.Email))
        {
            logger.LogWarning("Admin user already exists with Email: {Email}", request.Email);
            throw new AlreadyExist($"Email {request.Email} is already an Administrator.");
        }

        await adminRepository.AddPendingAsync(request.Email, admin.AdminId);
        logger.LogInformation("Successfully added a new pending admin with Email: {Email} by Admin: {AdminId}",
            request.Email, currentUser.Id);

    }
}

