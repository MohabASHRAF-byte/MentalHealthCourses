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
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

[AllowAnonymous]
[ApiController]
[SwaggerTag("\"Tenant\": just ignore sending tenant in any request",
    externalDocsUrl: "https://docs.google.com/document/d/1ZaLmmSHN9umbdhm02yTflUtidZUqavGue2xxfO4HNcw/edit?tab=t.0")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
[Route("api/admin")]
public class AdminIdentityController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(
        Summary = "Add Admin User",
        Description = AdminDocs.AddPendingAdminDescription)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("pending-admin")]
    public async Task<IActionResult> AddAdmin(AddAdminCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("pending-admin")]
    [SwaggerOperation(
        Summary = "Change pending admin Email",
        Description = AdminDocs.UpdatePendingAdmin
    )]
    public async Task<IActionResult> UpdatePendingAdmin(UpdatePendingAdminCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("pending-admin")]
    [SwaggerOperation(Summary = "Delete pending admin user.")]
    public async Task<IActionResult> DeletePendingAdmin(DeletePendingUsersCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("pending-admin")]
    [SwaggerOperation(Summary = "Get all pending admin users.")]
    public async Task<IActionResult> GetPendingAdmin([FromQuery] GetPendingUsersQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }


    [HttpPost("register")]
    [SwaggerOperation(
        Description = AdminDocs.RegisterAdminDescription,
        Summary = "Register a new admin user."
    )]
    public async Task<IActionResult> Register(RegisterAdminCommand command)
    {
        command.Tenant = Global.ProgramName;
        await mediator.Send(command);

        return Created();
    }

    [HttpPost("confirm-email")]
    [SwaggerOperation(Summary = "Confirm admin user's email.")]
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

    [HttpPost("resend-confirmation-email")]
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

    [HttpPost("forget-password")]
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

    [HttpPost("reset-password")]
    [SwaggerOperation(Description = IdentityDocs.ResetPasswordDescription, Summary = "Reset admin user password.")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        command.Tenant = Global.ProgramName;
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("change-password")]
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

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("roles")]
    [SwaggerOperation(Description = IdentityDocs.AddRolesDescription, Summary = "Add roles to admin user.")]
    public async Task<IActionResult> Roles(AddRolesCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("roles")]
    [SwaggerOperation(Description = IdentityDocs.RemoveRolesDescription, Summary = "Remove roles from admin user.")]
    public async Task<IActionResult> RemoveRoles(RemoveRolesCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }
}