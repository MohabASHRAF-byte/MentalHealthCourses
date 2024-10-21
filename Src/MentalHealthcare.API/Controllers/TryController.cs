using MediatR;
using MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;
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
}