using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers.Commands.Register;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Commands.Register;

public class RegisterAdminCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    IAdminRepository adminRepository,
    IEmailSender emailSender,
    UserManager<User> userManager,
    IMapper mapper
) : IRequestHandler<RegisterAdminCommand>
{
    public async Task Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering admin user with Email: {Email}", request.Email);

       

        if (!await adminRepository.IsPendingExistAsync(request.Email))
        {
            logger.LogWarning("Email {Email} is not registered as a pending admin.", request.Email);
            throw new ForBidenException($"{request.UserName} can't register with email {request.Email}");
        }

        var user = mapper.Map<User>(request);
        Admin admin = new()
        {
            User = user,
            FName = request.FirstName,
            LName = request.LastName,
        };
        user.Roles = Roles.AddRole(user.Roles, UserRoles.Admin);

        logger.LogInformation("Inserting user {UserName} into the database.", request.UserName);

        try
        {
            await adminRepository.RegisterUser(user, request.Password, admin);
            logger.LogInformation("Successfully registered user {UserName}.", request.UserName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to register user {UserName}.", request.UserName);
            throw;
        }

        await SendConfirmation(user);
    }

    private async Task SendConfirmation(User user)
    {
        logger.LogInformation("Sending confirmation email to {Email}.", user.Email);

        try
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await emailSender.SendEmailAsync(user.Email!, "Token to confirm your email", token);
            logger.LogInformation("Confirmation email sent successfully to {Email}.", user.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send confirmation email to {Email}.", user.Email);
            throw;
        }
    }
}
