using MediatR;
using MentalHealthcare.Application.BunnyServices.PodCast.Get;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Podcast.Episodes.Commands.AddNewEpisode;
using MentalHealthcare.Application.Podcast.Episodes.Commands.Delete;
using MentalHealthcare.Application.Podcast.Episodes.Commands.Update;
using MentalHealthcare.Application.Podcast.Episodes.Queries.GetAll;
using MentalHealthcare.Application.Podcast.Episodes.Queries.GetById;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create;
using MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetAll;
using MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetById;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
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
    public class PodCastController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Creates new PodCast",
                      Description = "Creates new PodCasts of Series with it's details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePodCast([FromForm] AddNewEpisodeCommand command)
        {
            var Episode = await mediator.Send(command);
            var op = OperationResult<object>
                .SuccessResult(new
                {
                    Episode = Episode
                });
            return Ok(op);
        }







        [HttpGet("{PodcastById}")]
        [SwaggerOperation(Summary = "Gets Episode by ID")]
        [ProducesResponseType(typeof(PodCastDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPodcastSeriesById([FromRoute] int PodcastById)
        {
            var query = new GetPodcastByIdQuery()
            {
                PodCastid = PodcastById
            };
            var podcast = await mediator.Send(query);
            var op = OperationResult<PodCastDto>
                .SuccessResult(podcast);
            return Ok(op);
        }






        [HttpGet("all")]
        [SwaggerOperation(Summary = "Gets all Episodes with pagination and search")]
        [ProducesResponseType(typeof(PageResult<PodCastDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPodcastSeries([FromQuery] GetAllEpisodesQuery query)
        {
            var Episodes = await mediator.Send(query);
            var result = OperationResult<PageResult<PodCastDto>>.SuccessResult(Episodes);
            return Ok(result);
        }




        [SwaggerOperation(Summary = "Delete The Podcast Episode")]
        [HttpDelete("{EpisodeId}")]
        public async Task<IActionResult> DeletePodcastEpisode([FromRoute] int EpisodeId)
        {
            var command = new DeleteEpisodeCommand()
            {
                EpisodeId = EpisodeId
            };
            await mediator.Send(command);
            return NoContent();
        }




        [SwaggerOperation(Summary = "Update Existing Podcast Episode")]
        [HttpPut("{EpisodeId}")]
        public async Task<IActionResult> UpdatePodcastEpisode([FromRoute] int EpisodeId,
    [FromForm] UpdateEpisodesCommand command)
        {
            command.PodcastId = EpisodeId;
            var episode = await mediator.Send(command);
            var op = OperationResult<object>
                .SuccessResult(new { EpisodeId = episode });
            return Ok(op);
        }






    }
}
