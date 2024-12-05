using MediatR;
using MentalHealthcare.Application.Articles.Commands.AddArticle;
using MentalHealthcare.Application.Articles.Commands.DeleteArticle;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Articles.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Meditations.Command.Delete;
using MentalHealthcare.Application.Meditations.Queries.GetAll;
using MentalHealthcare.Application.Meditations.Queries.GetById;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeditationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeditationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Meditation", Description = "Creates new Article with its details")]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> AddMeditation([FromForm] AddMeditationCommand command)
        {
            var MeditationID = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMeditationByIdQuery), new { MeditationID }, null);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Meditation")]
        [ProducesResponseType(typeof(PageResult<MeditationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMeditation([FromQuery] GetAllMeditationsQuery query)
        {
            var Meditation = await _mediator.Send(query);
            return Ok(Meditation);
        }


        [HttpGet("{MeditationId}")]
        [SwaggerOperation(Summary = "Get the Meditation by its ID")]
        [ProducesResponseType(typeof(MeditationDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeditationById([FromRoute] int MeditationId)
        {
            var query = new GetMeditationByIdQuery { Id = MeditationId };
            var Meditation = await _mediator.Send(query);
            return Ok(Meditation);
        }

        [HttpDelete("{MeditationId}")]
        [SwaggerOperation(Summary = "Delete Meditation by its ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMeditation([FromRoute] int MeditationId)
        {
            var command = new DeleteMeditationCommand { MeditationId = MeditationId };
            await _mediator.Send(command);
            return NoContent();
        }



















    }
}
