using MediatR;
using Microsoft.Extensions;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.SystemUsers.Commands.Register;

public class RegisterCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    IUserRepository userRepository,
    IEmailSender emailSender,
    UserManager<User> userManager,
    IMapper mapper ,
    IConfiguration configuration
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


        // Upload profile picture using Bunny service
        logger.LogInformation("Uploading Profile Picture For User: {UserName}", request.UserName);
        var bunny = new BunnyClient(configuration);

        var uniqueFileName = $"{request.UserName}_{Guid.NewGuid()}{Path.GetExtension(request.ProfilePic.FileName)}";

        var imageResponse = await bunny.UploadFileAsync(
            request.ProfilePic, uniqueFileName, "Users"
        );

        if (!imageResponse.IsSuccessful)
        {
            logger.LogWarning("Failed to upload ProfilePic for User: {UserName}. Error: {ErrorMessage}",
                request.UserName, imageResponse.Message);
            return OperationResult<UserDto>.Failure("Failed to upload profile picture. Please try again.");
        }

        await bunny.ClearCacheAsync(imageResponse.Url!);
















        User user = mapper.Map<User>(request);
        user.Roles = Roles.AddRole(user.Roles, UserRoles.User);
        SystemUser systemUser = mapper.Map<SystemUser>(request);
        systemUser.User = user;
        systemUser.FName = request.FirstName;
        systemUser.LName = request.LastName;
        systemUser.CreatedDate = DateTime.UtcNow;
        systemUser.LastUpdatedDate = DateTime.UtcNow;
        systemUser.BirthDate = request.Birthday;

        logger.LogInformation("Inserting {@user} to the DB", request.UserName);
        var isInserted = await userRepository.RegisterUser(user, request.Password, systemUser);
        if (!isInserted.Succeeded)
        {
            logger.LogInformation("User {@user} already exists", request.UserName);
            var ret = OperationResult<UserDto>.Failure("Try Again", StateCode.BadRequest);
            ret.Errors.AddRange(isInserted.Errors);
            return ret;
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