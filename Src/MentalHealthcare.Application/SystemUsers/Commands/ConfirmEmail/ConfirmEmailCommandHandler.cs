using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;

/// <summary>
/// Handles the process of confirming a user's email.
/// </summary>
/// <param name="request">The command containing the user's email and token for confirmation.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>An <see cref="OperationResult{T}"/> indicating the result of the email confirmation.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the start of the email confirmation process for the provided email.</description>
/// </item>
/// <item>
/// <description>Validate the tenant information: 
/// <list type="bullet">
/// <item>If the tenant is invalid (null or empty), log the issue and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Retrieve the user by their email and tenant: 
/// <list type="bullet">
/// <item>If the user does not exist, throw an exception indicating the user does not exist.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Log the authentication process for the email confirmation token.</description>
/// </item>
/// <item>
/// <description>Confirm the email using the token: 
/// <list type="bullet">
/// <item>If the confirmation fails, log a warning and throw an exception indicating the failure.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Log success confirmation of the email and return a successful operation result.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ResourceNotFound">
/// Thrown if the user with the specified email does not exist or if email confirmation fails.
/// </exception>
/// <exception cref="InvalidOperationException">
/// Thrown if the tenant is null or empty, indicating a bad request.
/// </exception>
public class ConfirmEmailCommandHandler(
    ILogger<ConfirmEmailCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository
) : IRequestHandler<ConfirmEmailCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Confirming email: {Email}", request.Email);

        // Validate tenant information
        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogWarning("Invalid Tenant information provided for email confirmation.");
            return OperationResult<string>.Failure("Bad Request", StateCode.BadRequest);
        }

        // Retrieve user by email and tenant
        var user = await systemUserRepository.GetUserByEmailAsync(request.Email, request.Tenant!);
        if (user == null)
        {
            logger.LogError("User with email {Email} does not exist.", request.Email);
            throw new ResourceNotFound(nameof(user), request.Email);
        }

        logger.LogInformation("Authenticating the token for email: {Email}", request.Email);
        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to authenticate the token for email: {Email}", request.Email);
            throw new ApplicationException($"Failed to confirm email: {request.Token}");
        }

        logger.LogInformation("Email {Email} confirmed successfully.", request.Email);
        return OperationResult<string>.SuccessResult("Email confirmed successfully", "Email confirmed successfully");
    }
}
