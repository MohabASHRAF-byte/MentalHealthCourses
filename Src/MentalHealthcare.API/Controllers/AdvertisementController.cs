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
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]


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
        var op = OperationResult<object>
            .SuccessResult(new
            {
                advertisementId = advertisementId
            });
        return Ok(op);
    }
    [SwaggerOperation(Summary="Get all Advertisements",
        Description = AdvertisementControllerDocs.GetAllDescription)]
    [ProducesResponseType(typeof(PageResult<AdvertisementDto>), 200)]  
    [HttpGet]
    public async Task<IActionResult> GetAllAdvertisements([FromQuery]GetAllAdvertisementsQuery query)
    {
        var advertisements = await mediator.Send(query);
        var op = OperationResult<PageResult<AdvertisementDto> >
            .SuccessResult(advertisements);
        return Ok(op);
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
        var op = OperationResult<AdvertisementDto>
            .SuccessResult(advertisement);
        return Ok(op);
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
        var op = OperationResult<object>
            .SuccessResult(new { advertisementId = advertisement });
        return Ok(op);
    }
}