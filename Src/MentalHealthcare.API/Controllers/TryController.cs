using System.Security.Claims;
using MediatR;
using MentalHealthcare.Application.BunnyServices.PodCast.Get;
using MentalHealthcare.Application.BunnyServices.UploadFile;
using MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video.CreateVideo;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video.Delete;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using MentalHealthcare.Domain.Constants;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TryController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [Route("checkInfo")]
    public async Task<IActionResult> ResendConfirmation([FromBody] AddCollectionCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }

    [HttpGet]
    [Route("any")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Try()
    {
        return Ok("You're Authorized");
    }

    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFile(CreateVideoCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }
    
}