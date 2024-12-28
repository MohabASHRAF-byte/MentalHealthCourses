using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;
[AllowAnonymous]

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiExplorerSettings(GroupName = Global.AllVersion)]


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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAdvertisement([FromForm] CreateAdvertisementCommand command)
    {
        var advertisementId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId = advertisementId }, null);
    }
    [SwaggerOperation(Summary="Get all Advertisements",
        Description = AdvertisementControllerDocs.GetAllDescription)]
    [ProducesResponseType(typeof(PageResult<AdvertisementDto>), 200)]  
    [HttpGet]
    public async Task<IActionResult> GetAllAdvertisements([FromQuery]GetAllAdvertisementsQuery query)
    {
        var advertisements = await mediator.Send(query);
        return Ok(advertisements);
    }
    [SwaggerOperation(Summary = "Get the add detailed with it's id",
        Description = AdvertisementControllerDocs.GetByIdDescription
        )]
    [ProducesResponseType(typeof(AdvertisementDto), 200)]  

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

    [SwaggerOperation(
        Summary = "Updated Existing Advertisement",
        Description = AdvertisementControllerDocs.UpdateDescription)]
    [HttpPut("{advertisementId}")]
    public async Task<IActionResult> UpdateAdvertisement([FromRoute] int advertisementId,
        [FromForm] UpdateAdvertisementCommand command)
    {
        command.AdvertisementId = advertisementId;
        var advertisement = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId = advertisement }, null);
    }
}