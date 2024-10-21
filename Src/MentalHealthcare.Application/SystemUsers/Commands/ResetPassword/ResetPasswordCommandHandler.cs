using MediatR;
using MentalHealthcare.Application.Background_Services;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    ILogger<ResetPasswordCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository,
    OtpService otpService
) : IRequestHandler<ResetPasswordCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // Find user by email
        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant ");
            return OperationResult<string>.Failure("Bad Request", StateCode.BadRequest);
        }
        var user = await systemUserRepository.GetUserByEmailAsync(request.Email, request.Tenant);
        if (user == null)
        {
            return OperationResult<string>.Failure("Invalid email");
        }
        // Verify OTP
        var otp = otpService.GetOtp(request.Email, request.Tenant);
        if (otp == null || otp != request.ResetCode)
        {
            return OperationResult<string>.Failure("Invalid or expired OTP");
        }

        // Reset password without requiring old password
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetResult = await userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

        if (!resetResult.Succeeded)
        {
            logger.LogError("Password reset failed for user {Email}. Errors: {Errors}", request.Email, resetResult.Errors);
            return OperationResult<string>.Failure("Failed to reset password");
        }

        // Successfully reset password
        return OperationResult<string>.SuccessResult(null, "Password reset successful");
    }
}