using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdvertisementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Advertisement", Description = "Creates new Advertisement with its details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAdvertisement([FromForm] CreateAdvertisementCommand command)
        {
            var advertisementId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId }, null);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Advertisements")]
        [ProducesResponseType(typeof(PageResult<AdvertisementDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAdvertisements([FromQuery] GetAllAdvertisementsQuery query)
        {
            var advertisements = await _mediator.Send(query);
            return Ok(advertisements);
        }

        [HttpGet("{advertisementId}")]
        [SwaggerOperation(Summary = "Get the advertisement by its ID")]
        [ProducesResponseType(typeof(AdvertisementDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdvertisementById([FromRoute] int advertisementId)
        {
            var query = new GetAdvertisementByIdQuery { AdvertisementId = advertisementId };
            var advertisement = await _mediator.Send(query);
            return Ok(advertisement);
        }

        [HttpDelete("{advertisementId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAdvertisement([FromRoute] int advertisementId)
        {
            var command = new DeleteAdvertisementCommand { AdvertisementId = advertisementId };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{advertisementId}")]
        [SwaggerOperation(Summary = "Update Existing Advertisement")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateAdvertisement([FromRoute] int advertisementId, [FromForm] UpdateAdvertisementCommand command)
        {
            command.AdvertisementId = advertisementId;
            var advertisement = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId = advertisement }, null);
        }
    }
}