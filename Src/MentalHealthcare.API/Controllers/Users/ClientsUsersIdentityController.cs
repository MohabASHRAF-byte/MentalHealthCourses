using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;
using MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;
using MentalHealthcare.Application.SystemUsers.Commands.ForgetPassword;
using MentalHealthcare.Application.SystemUsers.Commands.Login;
using MentalHealthcare.Application.SystemUsers.Commands.Refresh;
using MentalHealthcare.Application.SystemUsers.Commands.Register;
using MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;
using MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;
using MentalHealthcare.Application.SystemUsers.Commands.ValidateChangePasswordOtp;
using MentalHealthcare.Application.SystemUsers.Queries.GerUsersQuery;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Users;

[ApiController]
[Route("api/user")]
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
public class ClientsUsersIdentityController(
    IMediator mediator
) : ControllerBase
{
    // Registration and Email Confirmation
    [HttpPost("register")]
    [SwaggerOperation(Description = ClientIdentityDocs.RegisterDescription)]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Created => Ok(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    [HttpPost("confirmEmail")]
    [SwaggerOperation(Description = ClientIdentityDocs.ConfirmEmailDescription)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    [HttpPost("resendConfirmationEmail")]
    public async Task<IActionResult> ResendConfirmationEmail(ResendEmailConfirmationCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            StateCode.NotFound => NotFound(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    // Authentication
    [HttpPost("login")]
    [SwaggerOperation(Description = IdentityDocs.LoginDescription)]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshCommand command)
    {
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            StateCode.Unauthorized => Unauthorized(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    // Password Management
    [HttpPost("forgetPassword")]
    [SwaggerOperation(Description = ClientIdentityDocs.ForgetPasswordDescription)]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            StateCode.NotFound => NotFound(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    [HttpPost("resetPassword")]
    [SwaggerOperation(Description = IdentityDocs.ResetPasswordDescription)]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [HttpPost("validatePasswordChangeOtp")]
    [SwaggerOperation(Summary = "Validate OTP for password reset.")]
    public async Task<IActionResult> ValidatePasswordChangeOtp(ValidateChangePasswordOtpCommand command)
    {
        command.Tenant = Global.ApplicationTenant;
        await mediator.Send(command);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("changePassword")]
    [SwaggerOperation(Description = IdentityDocs.ChangePasswordDescription)]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            StateCode.NotFound => NotFound(commandResult),
            _ => BadRequest(commandResult)
        };
    }

}