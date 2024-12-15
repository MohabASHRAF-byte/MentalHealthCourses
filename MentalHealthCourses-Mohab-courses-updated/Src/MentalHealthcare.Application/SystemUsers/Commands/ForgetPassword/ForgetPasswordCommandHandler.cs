using MediatR;
using MentalHealthcare.Application.Background_Services;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ForgetPassword;

/// <summary>
/// Handles the process of sending a password reset OTP to a user's email.
/// </summary>
/// <param name="request">The command containing the email and tenant information.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>An <see cref="OperationResult{string}"/> indicating the result of the OTP send operation.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the attempt to change the password for the provided email.</description>
/// </item>
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
/// <item>If the user is not found, log the invalid email attempt and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Generate a One-Time Password (OTP) for password reset.</description>
/// </item>
/// <item>
/// <description>Add or update the OTP in the OTP cleanup service.</description>
/// </item>
/// <item>
/// <description>Send the OTP to the user's email address.</description>
/// </item>
/// <item>
/// <description>Return a success result indicating that the OTP has been sent successfully.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ApplicationException">
/// Thrown if the email provided is invalid or if the user does not exist.
/// </exception>
public class ForgetPasswordCommandHandler(
    ILogger<ForgetPasswordCommandHandler> logger,
    IUserRepository systemUserRepository,
    IEmailSender emailSender,
    OtpService otpCleanupService
) : IRequestHandler<ForgetPasswordCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ForgetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Changing password for {Email}", request.Email);

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

        // Generate OTP for password reset
        var otp = GenerateOtp();
        
        // Add or update OTP in the OTP cleanup service
        otpCleanupService.AddOrUpdateOtp(request.Email, request.Tenant, otp);
        
        // Send OTP via email
        await emailSender.SendEmailAsync(request.Email, "Reset Password", otp);
        
        return OperationResult<string>.SuccessResult(null, "Password OTP sent successfully", StateCode.Ok);
    }

    private string GenerateOtp()
    {
        var random = new Random();
        var otp = random.Next(100000, 999999);
        return otp.ToString();
    }
}
