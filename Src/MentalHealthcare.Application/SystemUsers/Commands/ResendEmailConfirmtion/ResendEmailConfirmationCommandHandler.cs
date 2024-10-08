using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;

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
        logger.LogInformation("Resend email confirmation to {@Email}", request.Email);
        var user = await userRepository.GetUserByEmailAsync(request.Email,request.Tenant!);
        if (user == null)
        {
            logger.LogInformation("User {Email} not found", request.Email);
            return OperationResult<string>.Failure("No user found");
        }
        if (await userManager.IsEmailConfirmedAsync(user))
        {
            logger.LogInformation("User {Email} has  confirmed email", request.Email);
            return OperationResult<string>.Failure("This email is already confirmed");
        }
        logger.LogInformation("Sending confirmation email to {@user}", user.Email);
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        await emailSender.SendEmailAsync(user.Email!, "Token to confirm your Email ", token);
        return OperationResult<string>.SuccessResult("Email confirmation email sent");
    }
}