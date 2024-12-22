using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Articles.Commands.Create;
using MentalHealthcare.Application.Articles.Commands.Delete;
using MentalHealthcare.Application.Articles.Commands.Update;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Articles.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [SwaggerOperation(Summary = "Creates new Article",
        Description = "Creates new Article with it's details")
]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateArticle([FromForm] AddArticleCommand command)
        {
            var ArticletId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetArticleById), new { ArticletId = ArticletId }, null);
        }


        [SwaggerOperation(Summary = "Get adll Articles",
        Description = ArticleControllerDocs.GetAllDescription)]
        [ProducesResponseType(typeof(PageResult<ArticleDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllArticles([FromQuery] GetAllArticlesQuery query)
        {
            var Article = await _mediator.Send(query);
            return Ok(Article);
        }




        [SwaggerOperation(Summary = "Get the Article detailed with it's id",
       Description = ArticleControllerDocs.GetByIdDescription
       )]
        [ProducesResponseType(typeof(ArticleDto), 200)]

        [HttpGet("{ArticleId}")]
        public async Task<IActionResult> GetArticleById([FromRoute] int ArticleId)
        {
            var query = new GetArticleByIdQuery()
            {
                ArticleId = ArticleId
            };
            var TheArticleId = await _mediator.Send(query);
            return Ok(TheArticleId);
        }



        [HttpDelete("{ArticleId}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int articleId)
        {
            var command = new DeleteArticleCommand()
            {
                ArticleId = articleId
            };
            await _mediator.Send(command);
            return NoContent();
        }



        [SwaggerOperation(
        Summary = "Updated Existing Article",
        Description = ArticleControllerDocs.UpdateDescription)]
        [HttpPut("{ArticleId}")]
        public async Task<IActionResult> UpdateArticle([FromRoute] int ArticleId,
        [FromForm] UpdateArticleCommand command)
        {
            command.ArticleId = ArticleId;
            var article = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetArticleById), new { advertisementId = article }, null);
        }




    }
}
