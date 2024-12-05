using MediatR;
using MentalHealthcare.Application.Articles.Commands.AddArticle;
using MentalHealthcare.Application.Articles.Commands.DeleteArticle;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Articles.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Podcasts.Commands.Create;
using MentalHealthcare.Application.Podcasts.Commands.Delete;
using MentalHealthcare.Application.Podcasts.Queries.GetAll;
using MentalHealthcare.Application.Podcasts.Queries.GetById;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PodcastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PodcastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Podcast", Description = "Creates new Podcast with its details")]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> AddPodcast([FromForm] AddPodcastCommand command)
        {
            var PodcastID = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdPodcastsQuery), new { PodcastID }, null);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Podcasts")]
        [ProducesResponseType(typeof(PageResult<PodCastDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllArticles([FromQuery] GetAllPodcastsQuery query)
        {
            var PodCasts = await _mediator.Send(query);
            return Ok(PodCasts);
        }


        [HttpGet("{articleId}")]
        [SwaggerOperation(Summary = "Get the Podcast by its ID")]
        [ProducesResponseType(typeof(PodCastDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPodcastById([FromRoute] int PodcastId)
        {
            var query = new GetByIdPodcastsQuery { PodcastId = PodcastId };
            var podCast = await _mediator.Send(query);
            return Ok(podCast);
        }

        [HttpDelete("{PodcastId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePodcast([FromRoute] int podCastId)
        {
            var command = new DeletePodcastCommand { podcastId = podCastId };
            await _mediator.Send(command);
            return NoContent();
        }

















    }
}
