using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AdvertisementController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
            Summary = "Creates new Advertisement",
            Description = "Creates new Advertisement with it's details"
        )
    ]
   // https://GeneralCommitteeDev.b-cdn.net/Advertisements/3_0
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAdvertisement([FromForm] CreateAdvertisementCommand command)
    {
        var advertisementId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId = advertisementId }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAdvertisements([FromQuery]GetAllAdvertisementsQuery query)
    {
        var advertisements = await mediator.Send(query);
        return Ok(advertisements);
    }

    [HttpGet("{advertisementId}")]
    public async Task<IActionResult> GetAdvertisementById([FromRoute] int advertisementId)
    {
        var query = new GetAdvertisementByIdQuery()
        {
            AdvertisementId = advertisementId
        };
        var advertisement = await mediator.Send(query);
        return Ok(advertisement);
    }

    [HttpDelete("{advertisementId}")]
    public async Task<IActionResult> DeleteAdvertisement([FromRoute] int advertisementId)
    {
        var command = new DeleteAdvertisementCommand()
        {
            AdvertisementId = advertisementId
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{advertisementId}")]
    public async Task<IActionResult> UpdateAdvertisement([FromRoute] int advertisementId,
        [FromBody] UpdateAdvertisementCommand command)
    {
        command.AdvertisementId = advertisementId;
        var advertisement = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId = advertisement }, null);
    }
}