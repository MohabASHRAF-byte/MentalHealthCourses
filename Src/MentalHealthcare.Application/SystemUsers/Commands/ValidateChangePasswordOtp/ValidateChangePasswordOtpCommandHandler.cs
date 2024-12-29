using MediatR;
using MentalHealthcare.Application.Background_Services;
using MentalHealthcare.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ValidateChangePasswordOtp;

public class ValidateChangePasswordOtpCommandHandler(
    ILogger<ValidateChangePasswordOtpCommandHandler> logger,
    OtpService otpService
) : IRequestHandler<ValidateChangePasswordOtpCommand>
{
    public Task Handle(ValidateChangePasswordOtpCommand request, CancellationToken cancellationToken)
    {
        // Check if the tenant information is valid
        if (string.IsNullOrEmpty(request.Tenant))
        {
           throw new InvalidOtp();
        }

        // Verify the OTP
        var otp = otpService.GetOtp(request.Email, request.Tenant);
        if (otp == null || otp != request.ResetCode)
        {
            logger.LogInformation("Invalid or expired OTP for {Email}", request.Email);
            throw new InvalidOtp();
        }

        return Task.CompletedTask;
    }
}
