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

public class ForgetPasswordCommandHandler(
    ILogger<ForgetPasswordCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository,
    IEmailSender emailSender,
    OtpService otpCleanupService
    ) : IRequestHandler<ForgetPasswordCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ForgetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("change password for {@email}", request.Email);

        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant ");
            return OperationResult<string>.Failure("Bad Request", StateCode.BadRequest);
        }
        var user = await systemUserRepository.GetUserByEmailAsync(request.Email, request.Tenant);
        if (user == null)
        {
            logger.LogInformation("invalid email : {@email}", request.Email);
            return OperationResult<string>.Failure("Invalid email");
        }
        var otp = GenerateOtp();
        otpCleanupService.AddOrUpdateOtp(request.Email, request.Tenant, otp);
        await emailSender.SendEmailAsync(request.Email, "Reset Password", otp);
        return OperationResult<string>.SuccessResult(null, "Password otp sent successfully", StateCode.Ok);
    }
    private string GenerateOtp()
    {
        var random = new Random();
        var otp = random.Next(100000, 999999);
        return otp.ToString();
    }
}