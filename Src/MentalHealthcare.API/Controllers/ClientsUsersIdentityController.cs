// using MediatR;
// using MentalHealthcare.API.Docs;
// using MentalHealthcare.Application.SystemUsers.Commands.AddRoles;
// using MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;
// using MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;
// using MentalHealthcare.Application.SystemUsers.Commands.ForgetPassword;
// using MentalHealthcare.Application.SystemUsers.Commands.Login;
// using MentalHealthcare.Application.SystemUsers.Commands.Refresh;
// using MentalHealthcare.Application.SystemUsers.Commands.Register;
// using MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;
// using MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;
// using MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;
// using MentalHealthcare.Domain.Constants;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Swashbuckle.AspNetCore.Annotations;
//
// namespace MentalHealthcare.API.Controllers;
//
// [ApiController]
// [Route("api/")]
// public class ClientsUsersIdentityController(
//     IMediator mediator
// ) : ControllerBase
// {
//     [HttpPost(nameof(Register))]
//     public async Task<IActionResult> Register(RegisterCommand command)
//     {
//         command.Tenant = Global.ApplicationTenant;
//         var commandResult = await mediator.Send(command);
//         if (commandResult.StatusCode == StateCode.Created)
//             return Ok(commandResult);
//         return BadRequest(commandResult);
//     }
//
//     [HttpPost(nameof(ConfirmEmail))]
//     public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
//     {
//         command.Tenant = Global.ApplicationTenant;
//
//         var commandResult = await mediator.Send(command);
//         if (commandResult.StatusCode == StateCode.Ok)
//         {
//             return Ok(commandResult);
//         }
//
//         return BadRequest(commandResult);
//     }
//
//     [HttpPost(nameof(Login))]
//     [SwaggerOperation(Description = IdentityDocs.LoginDescription)]
//
//     public async Task<IActionResult> Login(LoginCommand command)
//     {
//         command.Tenant = Global.ApplicationTenant;
//
//         var commandResult = await mediator.Send(command);
//         return Ok(commandResult);
//     }
//
//     [HttpPost(nameof(Refresh))]
//     public async Task<IActionResult> Refresh(RefreshCommand command)
//     {
//         var commandResult = await mediator.Send(command);
//         return commandResult.StatusCode switch
//         {
//             StateCode.Ok => Ok(commandResult),
//             StateCode.Unauthorized => Unauthorized(commandResult),
//             _ => BadRequest(commandResult)
//         };
//     }
//
//     [HttpPost(nameof(ResendConfirmationEmail))]
//     public async Task<IActionResult> ResendConfirmationEmail(
//         ResendEmailConfirmationCommand command)
//     {
//         command.Tenant = Global.ApplicationTenant;
//
//         var commandResult = await mediator.Send(command);
//         return commandResult.StatusCode switch
//         {
//             StateCode.Ok => Ok(commandResult),
//             StateCode.NotFound => NotFound(commandResult),
//             _ => BadRequest(commandResult)
//         };
//     }
//
//     [HttpPost(nameof(ForgetPassword))]
//     public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
//     {
//         command.Tenant = Global.ApplicationTenant;
//
//         var commandResult = await mediator.Send(command);
//         return commandResult.StatusCode switch
//         {
//             StateCode.Ok => Ok(commandResult),
//             StateCode.NotFound => NotFound(commandResult),
//             _ => BadRequest(commandResult)
//         };
//     }
//     [SwaggerOperation(Description = IdentityDocs.ResetPasswordDescription)]
//
//     [HttpPost(nameof(ResetPassword))]
//     public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
//     {
//         command.Tenant = Global.ApplicationTenant;
//
//         var commandResult = await mediator.Send(command);
//         return Ok(commandResult);
//     }
//
//     [Authorize(AuthenticationSchemes = "Bearer")]
//     [HttpPost(nameof(ChangePassword))]
//     [SwaggerOperation(Description = IdentityDocs.ChangePasswordDescription)]
//
//     public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
//     {
//         var commandResult = await mediator.Send(command);
//
//         return commandResult.StatusCode switch
//         {
//             StateCode.Ok => Ok(commandResult),
//             StateCode.NotFound => NotFound(commandResult),
//             _ => BadRequest(commandResult)
//         };
//     }
//     
// }