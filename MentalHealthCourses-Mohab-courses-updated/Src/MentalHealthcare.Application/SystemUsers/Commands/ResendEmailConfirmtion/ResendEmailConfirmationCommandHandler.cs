using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;

/// <summary>
/// Handles the process of resending email confirmation for a user.
/// </summary>
/// <param name="request">The command containing the email and tenant information.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>An <see cref="OperationResult{string}"/> indicating the result of the resend operation.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the attempt to resend email confirmation.</description>
/// </item>
/// <item>
/// <description>Retrieve the user by email from the user repository.
/// <list type="bullet">
/// <item>If the user is not found, log the information and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Check if the user's email is already confirmed.
/// <list type="bullet">
/// <item>If confirmed, log the information and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Generate an email confirmation token for the user and send the confirmation email.</description>
/// </item>
/// <item>
/// <description>Return a success result indicating that the confirmation email has been sent.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ApplicationException">
/// Thrown if the user does not exist or the email is already confirmed.
/// </exception>
public class ResendEmailConfirmationCommandHandler(
    ILogger<ResendEmailConfirmationCommandHandler> logger,
    IEmailSender emailSender,
    UserManager<User> userManager,
    IUserRepository userRepository
) : IRequestHandler<ResendEmailConfirmationCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ResendEmailConfirmationCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Resending email confirmation to {Email}", request.Email);
        
        var user = await userRepository.GetUserByEmailAsync(request.Email, request.Tenant!);
        
        if (user == null)
        {
            logger.LogInformation("User {Email} not found", request.Email);
            return OperationResult<string>.Failure("No user found");
        }
        
        if (await userManager.IsEmailConfirmedAsync(user))
        {
            logger.LogInformation("User {Email} has confirmed email", request.Email);
            return OperationResult<string>.Failure("This email is already confirmed");
        }
        
        logger.LogInformation("Sending confirmation email to {Email}", user.Email);
        
        // Generate email confirmation token
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        // Send confirmation email
        await emailSender.SendEmailAsync(user.Email!, "Token to confirm your Email", token);
        
        return OperationResult<string>.SuccessResult("Email confirmation email sent");
    }
}
