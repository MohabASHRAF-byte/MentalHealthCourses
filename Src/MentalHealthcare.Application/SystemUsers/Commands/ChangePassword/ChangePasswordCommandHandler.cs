using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    ILogger<ChangePasswordCommandHandler> logger,
    UserManager<User> userManager,
    IUserContext userContext,
    IUserRepository systemUserRepository
) : IRequestHandler<ChangePasswordCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        // Fetch current user from context
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning("Change password attempt failed. User not logged in.");
            return OperationResult<string>.Failure("User not logged in");
        }

        // Ensure the user exists in the UserManager
        var user = await systemUserRepository.GetUserByUserNameAsync(currentUser.UserName, currentUser.Tenant);
        if (user == null)
        {
            logger.LogWarning("Change password failed. User {UserName} could not be found in UserManager.",
                currentUser.UserName);
            return OperationResult<string>.Failure("User not found");
        }

        // Attempt to change the password
        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogWarning("Password change failed for user {UserName}. Errors: {Errors}", currentUser.UserName,
                errorMessages);
            return OperationResult<string>.Failure("Failed to change password: " + errorMessages);
        }

        logger.LogInformation("Password successfully changed for user {UserName}.", currentUser.UserName);
        return OperationResult<string>.SuccessResult("Password changed successfully");
    }
}