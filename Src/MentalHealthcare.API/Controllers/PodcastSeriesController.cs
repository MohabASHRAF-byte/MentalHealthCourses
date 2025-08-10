using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Application.Instructors.Commands.Delete;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Meditations.Queries.GetById;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Delete;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Update;
using MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetAll;
using MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetById;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
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
    //[Route("api/[controller]")]
    //[ApiController]
    public class PodcastSeriesController(IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Series",
                       Description = "Creates new Series of Podcasts with it's details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSeries([FromForm] AddPodcastSeriesCommand command)
        {
            var Series = await mediator.Send(command);
            var op = OperationResult<object>
                .SuccessResult(new
                {
                    Series = Series
                });
            return Ok(op);
        }




        [HttpGet("{PodcastSeriesById}")]
        [SwaggerOperation(Summary = "Gets Series by ID")]
        [ProducesResponseType(typeof(PodcastSeriesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPodcastSeriesById([FromRoute] int PodcastSeriesById)
        {
            var query = new GetPodcastSeriesByIdQuery()
            {
                PodcastSeriesId = PodcastSeriesById
            };
            var podcast = await mediator.Send(query);
            var op = OperationResult<PodcastSeriesDto>
                .SuccessResult(podcast);
            return Ok(op);
        }






        [SwaggerOperation(
        Summary = "Updated Existing PodcastSeries")]
        [HttpPut("{PodcastSeriesId}")]
        public async Task<IActionResult> UpdatePodcastSeries([FromRoute] int PodcastSeriesId,
        [FromForm] UpdatePodcastSeriesCommand command)
        {
            command.PodcastSeriesId = PodcastSeriesId;
            var ser = await mediator.Send(command);
            var op = OperationResult<object>
                .SuccessResult(new { PodcastSeriesId = ser });
            return Ok(op);
        }




        [SwaggerOperation(Summary = "Delete The PodcastSeries")]

        [HttpDelete("{PodcastSeriesId}")]
        public async Task<IActionResult> DeletePodcastSeries([FromRoute] int PodcastSeriesId)
        {
            var command = new DeletePodcastSeriesCommand()
            {
                SeriesId = PodcastSeriesId
            };
            await mediator.Send(command);
            return NoContent();
        }


        [HttpGet("all")]
        [SwaggerOperation(Summary = "Gets all Podcast Series with pagination and search")]
        [ProducesResponseType(typeof(PageResult<PodcastSeriesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPodcastSeries([FromQuery] GetAllPodcastSeriesQuery query)
        {
            var series = await mediator.Send(query);
            var result = OperationResult<PageResult<PodcastSeriesDto>>.SuccessResult(series);
            return Ok(result);
        }






    }










}
