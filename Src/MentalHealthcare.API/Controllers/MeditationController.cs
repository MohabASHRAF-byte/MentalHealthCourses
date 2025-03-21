using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Meditations.Command.Delete;
using MentalHealthcare.Application.Meditations.Command.Update;
using MentalHealthcare.Application.Meditations.Queries.GetAll;
using MentalHealthcare.Application.Meditations.Queries.GetById;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [AllowAnonymous]

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]
    public class MeditationController(IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new Meditation", Description = "Creates a new meditation session with details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateMeditation([FromForm] CreateMeditationCommand command)
        {
            var medid = await mediator.Send(command);
            var op = OperationResult<object>
                .SuccessResult(new
                {
                    medid = medid
                });
            return Ok(op);
        }

        [HttpGet("{meditationId}")]
        [SwaggerOperation(Summary = "Gets Meditation by ID")]
        [ProducesResponseType(typeof(MeditationDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeditationById([FromRoute] int meditationId)
        {
            var query = new GetMeditationByIdQuery()
            {
                Meditationid = meditationId
            };
            var meditation = await mediator.Send(query);
            var op = OperationResult<MeditationDto>
                .SuccessResult(meditation);
            return Ok(op);
        }

        [HttpPut("{meditationId}")]
        [SwaggerOperation(Summary = "Updates an existing Meditation session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMeditation([FromRoute] int meditationId, [FromForm] UpdateMeditationCommand command)
        {
            command.Meditationid = meditationId;
            var updatedMeditation = await mediator.Send(command);
            return Ok(updatedMeditation);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all Meditations")]
        [ProducesResponseType(typeof(PageResult<MeditationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMeditations([FromQuery] GetAllMeditationsQuery query)
        {
            var meditations = await mediator.Send(query);
            return Ok(meditations);
        }

        [HttpDelete("{meditationId}")]
        [SwaggerOperation(Summary = "Deletes a Meditation session by ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMeditation([FromRoute] int meditationId)
        {
            var command = new DeleteMeditationCommand { MeditationID = meditationId };
            await mediator.Send(command);
            return NoContent();
        }












    }
}
