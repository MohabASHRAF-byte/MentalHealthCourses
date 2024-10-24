using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.Register;

public class RegisterCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    IUserRepository userRepository,
    IEmailSender emailSender,
    UserManager<User> userManager,
    IMapper mapper
) : IRequestHandler<RegisterCommand, OperationResult<UserDto>>
{
    public async Task<OperationResult<UserDto>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Register systemUser with Email : {@user}", request.Email);

        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant");

            return OperationResult<UserDto>.Failure("Bad Request");
        }

        User user = mapper.Map<User>(request);
        user.Roles = Roles.AddRole(user.Roles, UserRoles.User);
        SystemUser systemUser = mapper.Map<SystemUser>(request);
        systemUser.User = user;
        systemUser.FName = request.FirstName;
        systemUser.LName = request.LastName;
        
        logger.LogInformation("Inserting {@user} to the DB", request.UserName);
        var isInserted = await userRepository.RegisterUser(user, request.Password, systemUser);
        if (!isInserted)
        {
            logger.LogInformation("User {@user} already exists", request.UserName);
            return OperationResult<UserDto>.Failure("User Already Exist", StateCode.BadRequest);
        }

        logger.LogError("User {@user} Registered successfully", request.UserName);
        await SendConfirmation(user);
        var userDto = mapper.Map<UserDto>(request);
        logger.LogInformation("User {@user} Registered successfully", request.UserName);
        return OperationResult<UserDto>.SuccessResult(userDto, "User Created Successfully", StateCode.Created);
    }

    private async Task SendConfirmation(User user)
    {
        logger.LogInformation("Sending confirmation email to {@user}", user.Email);
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        await emailSender.SendEmailAsync(user.Email!, "Token to confirm your Email ", token);
    }
}