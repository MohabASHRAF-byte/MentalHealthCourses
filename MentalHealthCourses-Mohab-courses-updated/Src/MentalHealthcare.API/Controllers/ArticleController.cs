using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Articles.Commands.AddArticle;
using MentalHealthcare.Application.Articles.Commands.DeleteArticle;
using MentalHealthcare.Application.Articles.Commands.UpdateArticle;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Articles.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {


        private readonly IMediator _mediator;

        public ArticleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Article", Description = "Creates new Article with its details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddArticle([FromForm] AddArticleCommand command)
        {
            var ArticleID = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetArticleByIdQuery), new { ArticleID }, null);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Articles")]
        [ProducesResponseType(typeof(PageResult<ArticleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllArticles([FromQuery] GetAllArticlesQuery query)
        {
            var articles = await _mediator.Send(query);
            return Ok(articles);
        }

        [HttpGet("{articleId}")]
        [SwaggerOperation(Summary = "Get the Article by its ID")]
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetArticleById([FromRoute] int ArticleId)
        {
            var query = new GetArticleByIdQuery { Id = ArticleId };
            var article = await _mediator.Send(query);
            return Ok(article);
        }

        [HttpDelete("{articleId}")]
        [SwaggerOperation(Summary = "Delete the Article by Taking ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteArticle([FromRoute] int ArticleId)
        {
            var command = new DeleteArticleCommand { ArticleId = ArticleId };
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpPut]
        [SwaggerOperation(Summary = "Update Any Detail Of Article")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateArticle(UpdateArticleCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }


    }
}
