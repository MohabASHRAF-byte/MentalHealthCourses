using MediatR;
using MentalHealthcare.Application.AdminUsers.Commands.Add;
using MentalHealthcare.Application.AdminUsers.Commands.Delete;
using MentalHealthcare.Application.AdminUsers.Commands.Register;
using MentalHealthcare.Application.AdminUsers.Commands.Update;
using MentalHealthcare.Application.AdminUsers.Queries;
using MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;
using MentalHealthcare.Application.SystemUsers.Commands.AddRoles;
using MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;
using MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;
using MentalHealthcare.Application.SystemUsers.Commands.ForgetPassword;
using MentalHealthcare.Application.SystemUsers.Commands.Login;
using MentalHealthcare.Application.SystemUsers.Commands.Refresh;
using MentalHealthcare.Application.SystemUsers.Commands.Register;
using MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;
using MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;
using MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("admin/")]
public class AdminIdentityController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("PendingAdmin")]
    public async Task<IActionResult> AddAdmin(AddAdminCommand command)
    {
        await mediator.Send(command);
        return Created();
    }
    [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpPut("PendingAdmin")]
    public async Task<IActionResult> UpdatePendingAdmin(UpdatePendingAdminCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpDelete("PendingAdmin")]
    public async Task<IActionResult> DeletePendingAdmin(DeletePendingUsersCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpGet("PendingAdmin")]
    public async Task<IActionResult> GetPendingAdmin([FromQuery] GetPendingUsersQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(RegisterAdminCommand command)
    {
        command.Tenant = Global.ProgramName;
        await mediator.Send(command);
        return Ok("Success");
    }

    [HttpPost(nameof(ConfirmEmail))]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        if (commandResult.StatusCode == StateCode.Ok)
        {
            return Ok(commandResult);
        }

        return BadRequest(commandResult);
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [HttpPost(nameof(Refresh))]
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

    [HttpPost(nameof(ResendConfirmationEmail))]
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

    [HttpPost(nameof(ForgetPassword))]
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

    [HttpPost(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost(nameof(ChangePassword))]
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

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost(nameof(Roles))]
    public async Task<IActionResult> Roles(AddRolesCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("Roles")]
    public async Task<IActionResult> RemoveRoles(RemoveRolesCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }
}