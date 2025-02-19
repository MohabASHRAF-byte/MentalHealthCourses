using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.AdminUsers.Commands.Add;
using MentalHealthcare.Application.AdminUsers.Commands.Delete;
using MentalHealthcare.Application.AdminUsers.Commands.Register;
using MentalHealthcare.Application.AdminUsers.Commands.Update;
using MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;
using MentalHealthcare.Application.SystemUsers.Commands.AddRoles;
using MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;
using MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;
using MentalHealthcare.Application.SystemUsers.Commands.ForgetPassword;
using MentalHealthcare.Application.SystemUsers.Commands.Login;
using MentalHealthcare.Application.SystemUsers.Commands.Refresh;
using MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;
using MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;
using MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;
using MentalHealthcare.Application.SystemUsers.Commands.ValidateChangePasswordOtp;
using MentalHealthcare.Application.SystemUsers.Queries.GerUsersQuery;
using MentalHealthcare.Application.SystemUsers.Queries.GetUserQuery;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Users;

[ApiController]
[Route("api/admin")]
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
public class AdminIdentityController(
    IMediator mediator
) : ControllerBase
{
    // Registration and Email Confirmation
    [HttpPost("register")]
    [SwaggerOperation(Description = AdminDocs.RegisterAdminDescription, Summary = "Register a new admin user.")]
    public async Task<IActionResult> Register(RegisterAdminCommand command)
    {
        command.Tenant = Global.ProgramName;
        await mediator.Send(command);
        return Created();
    }

    [HttpPost("confirmEmail")]
    [SwaggerOperation(Summary = "Confirm admin user's email.")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    [HttpPost("resendConfirmationEmail")]
    [SwaggerOperation(Summary = "Resend confirmation email to admin user.")]
    public async Task<IActionResult> ResendConfirmationEmail(ResendEmailConfirmationCommand command)
    {
        command.Tenant = Global.ProgramName;
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
    [SwaggerOperation(Description = IdentityDocs.LoginDescription, Summary = "Login admin user.")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refresh admin user token.")]
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
    [SwaggerOperation(Summary = "Initiate admin user password reset.")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return commandResult.StatusCode switch
        {
            StateCode.Ok => Ok(commandResult),
            StateCode.NotFound => NotFound(commandResult),
            _ => BadRequest(commandResult)
        };
    }

    [HttpPost("resetPassword")]
    [SwaggerOperation(Description = IdentityDocs.ResetPasswordDescription, Summary = "Reset admin user password.")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [HttpPost("validatePasswordChangeOtp")]
    [SwaggerOperation(Summary = "Validate OTP for password reset.")]
    public async Task<IActionResult> ValidatePasswordChangeOtp(ValidateChangePasswordOtpCommand command)
    {
        command.Tenant = Global.ProgramName;
        await mediator.Send(command);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("changePassword")]
    [SwaggerOperation(Description = IdentityDocs.ChangePasswordDescription, Summary = "Change admin user password.")]
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

    // Roles Management
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("Admin")]
    public async Task<IActionResult> AddRoles(AddRolesCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("admin")]
    [SwaggerOperation(Description = IdentityDocs.RemoveRolesDescription, Summary = "Remove roles from admin user.")]
    public async Task<IActionResult> RemoveRoles(RemoveRolesCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    // Pending Admin Management
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("pendingAdmin")]
    [SwaggerOperation(Summary = "Add pending admin user.")]
    public async Task<IActionResult> AddPendingAdmin(AddAdminCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("pendingAdmin")]
    [SwaggerOperation(Summary = "Update pending admin user.")]
    public async Task<IActionResult> UpdatePendingAdmin(UpdatePendingAdminCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("pendingAdmin")]
    [SwaggerOperation(Summary = "Delete pending admin user.")]
    public async Task<IActionResult> DeletePendingAdmin(DeletePendingUsersCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("pendingAdmin")]
    [SwaggerOperation(Summary = "Get all pending admin users.")]
    public async Task<IActionResult> GetPendingAdmin([FromQuery] GetPendingUsersQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetUsersQuery query)
    {
        var results = await mediator.Send(query);
        return Ok(results);
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] int userId)
    {
        var query = new GetUserQuery()
        {
            UserId = userId
        };
        var results = await mediator.Send(query);
        return Ok(results);
    }
}