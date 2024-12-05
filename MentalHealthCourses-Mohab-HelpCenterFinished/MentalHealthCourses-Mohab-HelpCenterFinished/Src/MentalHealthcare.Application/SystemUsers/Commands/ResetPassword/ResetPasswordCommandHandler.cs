using MediatR;
using MentalHealthcare.Application.Background_Services;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;

/// <summary>
/// Handles the process of resetting a user's password.
/// </summary>
/// <param name="request">The command containing the email, tenant, reset code, and new password.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>An <see cref="OperationResult{string}"/> indicating the result of the password reset operation.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Check if the tenant information is valid.
/// <list type="bullet">
/// <item>If invalid, log the information and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Retrieve the user by email from the user repository.
/// <list type="bullet">
/// <item>If the user does not exist, return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Verify the One-Time Password (OTP) using the OTP service.
/// <list type="bullet">
/// <item>If the OTP is invalid or expired, return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Generate a password reset token for the user.</description>
/// </item>
/// <item>
/// <description>Attempt to reset the password using the generated token and the new password.
/// <list type="bullet">
/// <item>If the reset fails, log the error and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Return a success result indicating that the password was reset successfully.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ApplicationException">
/// Thrown if the email is invalid or if the OTP verification fails.
/// </exception>
public class ResetPasswordCommandHandler(
    ILogger<ResetPasswordCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository,
    OtpService otpService
) : IRequestHandler<ResetPasswordCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // Check if the tenant information is valid
        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant");
            return OperationResult<string>.Failure("Bad Request", StateCode.BadRequest);
        }

        // Retrieve the user by email
        var user = await systemUserRepository.GetUserByEmailAsync(request.Email, request.Tenant);
        
        // Check if the user exists
        if (user == null)
        {
            logger.LogInformation("Invalid email: {Email}", request.Email);
            return OperationResult<string>.Failure("Invalid email");
        }

        // Verify the OTP
        var otp = otpService.GetOtp(request.Email, request.Tenant);
        if (otp == null || otp != request.ResetCode)
        {
            logger.LogInformation("Invalid or expired OTP for {Email}", request.Email);
            return OperationResult<string>.Failure("Invalid or expired OTP");
        }

        // Generate a password reset token for the user
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        
        // Attempt to reset the password
        var resetResult = await userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
        
        // Check if the reset operation succeeded
        if (!resetResult.Succeeded)
        {
            logger.LogError("Password reset failed for user {Email}. Errors: {Errors}", request.Email, resetResult.Errors);
            return OperationResult<string>.Failure("Failed to reset password");
        }

        // Successfully reset password
        return OperationResult<string>.SuccessResult(null, "Password reset successful");
    }
}
