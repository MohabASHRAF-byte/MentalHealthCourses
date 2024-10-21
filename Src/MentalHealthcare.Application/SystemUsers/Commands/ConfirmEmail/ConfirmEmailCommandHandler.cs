using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler(
    ILogger<ConfirmEmailCommandHandler> logger,
    UserManager<User> userManager,
    IUserRepository systemUserRepository
) : IRequestHandler<ConfirmEmailCommand, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Confirming {@email}", request.Email);
        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant ");
            return OperationResult<string>.Failure("Bad Request", StateCode.BadRequest);
        }
        var user = await systemUserRepository.GetUserByEmailAsync(request.Email, request.Tenant!);
        if (user == null)
            throw new ApplicationException($"User with email {request.Email} does not exist");

        logger.LogInformation("Authenticate the token for {@email}", request.Email);
        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to authenticate the token for {@email}", request.Email);
            throw new ApplicationException($"Failed to confirm email: {request.Token}");
        }

        logger.LogInformation("{@email} confirmed", request.Email);
        return OperationResult<string>
            .SuccessResult("Email Confirmed successfully", "Email Confirmed successfully");
    }
}