using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;
using MentalHealthcare.Application.Utitlites.Jwt;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.Login;

public class LoginCommandHandler(
    ILogger<ConfirmEmailCommandHandler> logger,
    UserManager<User> userManager,
    IEmailSender emailSender,
    IJwtToken jwtToken,
    IUserRepository userRepository
) : IRequestHandler<LoginCommand, OperationResult<LoginDto>>
{
    public async Task<OperationResult<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user;
        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant ");
            return OperationResult<LoginDto>.Failure("Bad Request", StateCode.BadRequest);
        }

        logger.LogInformation("Login Request for {@user}", request.UserIdentifier);
        if (request.UserIdentifier.IsValidEmail())
        {
            user = await userRepository.GetUserByEmailAsync(request.UserIdentifier, request.Tenant!);
        }
        else if (request.UserIdentifier.IsValidPhoneNumber())
        {
            user = await userRepository.GetUserByPhoneNumberAsync(request.UserIdentifier, request.Tenant!);
        }
        else
        {
            user = await userRepository.GetUserByUserNameAsync(request.UserIdentifier, request.Tenant!);
        }

        if (user == null)
            throw new ResourceNotFound("User", request.UserIdentifier);
        if (user.Tenant != request.Tenant)
        {
            logger.LogInformation("{@user} belongs to {@tenant1} not @{tenant2}"
                , request.UserIdentifier, user.Tenant, request.Tenant);
            return OperationResult<LoginDto>.Failure("Bad Request", StateCode.BadRequest);
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            logger.LogInformation("{@user} is not confirmed", user.Email);
            return OperationResult<LoginDto>.Failure("Bad Request", StateCode.BadRequest);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            logger.LogWarning("Invalid password for user {EmailOrUserName}", request.UserIdentifier);
            return OperationResult<LoginDto>.Failure("Un authorized", StateCode.Unauthorized);
        }

        if (await userManager.GetTwoFactorEnabledAsync(user))
        {
            logger.LogInformation("2FA is enabled for user {@user}. Sending 2FA code.",
                request.UserIdentifier);

            var otp = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await emailSender.SendEmailAsync(user.Email!, "Your 2FA Code", $"Your code is {otp}");
            return OperationResult<LoginDto>.SuccessResult(new LoginDto(), "2FA code sent");
            // TODO: Send OTP via phone
        }

        logger.LogInformation("Returning token for {@user}", user.UserName);
        var tokens = await jwtToken.GetTokens(user);
        return OperationResult<LoginDto>.SuccessResult(new LoginDto()
        {
            Name = user.UserName!,
            Token = tokens.Item1,
            RefreshToken = tokens.Item2
        }, "Success");
    }
}