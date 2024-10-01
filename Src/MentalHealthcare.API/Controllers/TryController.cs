using MediatR;
using MentalHealthcare.Application.BunnyServices.PodCast.Get;
using MentalHealthcare.Application.BunnyServices.UploadFile;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video.Delete;
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
    public async Task<IActionResult> ResendConfirmation([FromBody] UploadFileCommand command)
    {
        var commandResult = await mediator.Send(command);
        return Ok(commandResult);
    }
}