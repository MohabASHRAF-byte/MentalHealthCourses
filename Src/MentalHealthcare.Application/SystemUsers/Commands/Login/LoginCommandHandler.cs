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

/// <summary>
/// Handles the login process for users.
/// </summary>
/// <param name="request">The command containing the user's login credentials.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>An <see cref="OperationResult{LoginDto}"/> indicating the result of the login attempt.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Validate tenant information: 
/// <list type="bullet">
/// <item>If the tenant is invalid (null or empty), log the issue and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Log the login request for the provided user identifier.</description>
/// </item>
/// <item>
/// <description>Retrieve user based on the identifier: 
/// <list type="bullet">
/// <item>Check if the identifier is an email, phone number, or username, and fetch the corresponding user.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Check if the user exists: 
/// <list type="bullet">
/// <item>If the user does not exist, throw a ResourceNotFound exception.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Verify tenant association: 
/// <list type="bullet">
/// <item>If the user's tenant does not match the requested tenant, log the issue and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Check if the email is confirmed: 
/// <list type="bullet">
/// <item>If not confirmed, log the issue and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Validate the user's password: 
/// <list type="bullet">
/// <item>If the password is invalid, log a warning and return an unauthorized result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Check for two-factor authentication (2FA): 
/// <list type="bullet">
/// <item>If enabled, generate and send a 2FA code via email, and return a success message indicating the code was sent.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>If 2FA is not enabled, generate a JWT token for the user and return a success result with the token information.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ResourceNotFound">
/// Thrown if the user with the specified identifier does not exist.
/// </exception>
/// <exception cref="ApplicationException">
/// Thrown if the tenant is invalid or if the user’s email is not confirmed.
/// </exception>
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

        // Validate tenant information
        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogWarning("Invalid tenant information provided for login.");
            return OperationResult<LoginDto>.Failure("Bad Request", StateCode.BadRequest);
        }

        logger.LogInformation("Login request for user: {UserIdentifier}", request.UserIdentifier);

        // Retrieve user based on identifier
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

        // Check if the user exists
        if (user == null)
        {
            logger.LogError("User with identifier {UserIdentifier} not found.", request.UserIdentifier);
            throw new ResourceNotFound("User", request.UserIdentifier);
        }

        // Verify tenant association
        if (user.Tenant != request.Tenant)
        {
            logger.LogWarning("User {UserIdentifier} belongs to tenant {Tenant1} but attempted to log in with tenant {Tenant2}.",
                request.UserIdentifier, user.Tenant, request.Tenant);
            return OperationResult<LoginDto>.Failure("Bad Request", StateCode.BadRequest);
        }

        // Check if email is confirmed
        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            logger.LogWarning("User {Email} has not confirmed their email.", user.Email);
            return OperationResult<LoginDto>.Failure("Bad Request", StateCode.BadRequest);
        }

        // Validate password
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            logger.LogWarning("Invalid password attempt for user {UserIdentifier}.", request.UserIdentifier);
            return OperationResult<LoginDto>.Failure("Unauthorized", StateCode.Unauthorized);
        }

        // Check for two-factor authentication
        if (await userManager.GetTwoFactorEnabledAsync(user))
        {
            logger.LogInformation("2FA is enabled for user {UserIdentifier}. Sending 2FA code.",
                request.UserIdentifier);

            var otp = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await emailSender.SendEmailAsync(user.Email!, "Your 2FA Code", $"Your code is {otp}");
            return OperationResult<LoginDto>.SuccessResult(new LoginDto(), "2FA code sent");
            // TODO: Send OTP via phone
        }

        logger.LogInformation("Returning token for user {UserIdentifier}.", user.UserName);
        var tokens = await jwtToken.GetTokens(user);
        return OperationResult<LoginDto>.SuccessResult(new LoginDto()
        {
            Name = user.UserName!,
            Token = tokens.Item1,
            RefreshToken = tokens.Item2
        }, "Success");
    }
}
