using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
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
    IUserRepository userRepository,
    IAdminRepository adminRepository,
    IEmailSender emailSender,
    UserManager<User> userManager,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<RegisterAdminCommand>
{
    //todo add admin role
    public async Task Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Register systemUser with Email : {@user}", request.Email);


        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant");
            throw new Exception("Invalid Tenant");
        }

        if (request.Tenant != Global.ProgramName)
            throw new ForBidenException("Not allowed");

        if (!await adminRepository.IsPendingExistAsync(request.Email))
            throw new ForBidenException($"{request.UserName} Can't register with email {request.Email}");

        User user = mapper.Map<User>(request);
        Admin admin = new()
        {
            User = user,
            FName = request.FirstName,
            LName = request.LastName
        };
        user.Roles = Roles.AddRole(user.Roles, UserRoles.Admin);
        logger.LogInformation("Inserting {@user} to the DB", request.UserName);
        var isInserted = await adminRepository.RegisterUser(user, request.Password, admin);
        if (!isInserted)
        {
            logger.LogInformation("User {@user} already exists", request.UserName);
            throw new AlreadyExist($"{request.UserName} already exists");
        }

        logger.LogError("User {@user} Registered successfully", request.UserName);
        await adminRepository.DeletePendingAsync([request.Email]);
        await SendConfirmation(user);
        logger.LogInformation("User {@user} Registered successfully", request.UserName);
    }

    private async Task SendConfirmation(User user)
    {
        logger.LogInformation("Sending confirmation email to {@user}", user.Email);
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        await emailSender.SendEmailAsync(user.Email!, "Token to confirm your Email ", token);
    }
}