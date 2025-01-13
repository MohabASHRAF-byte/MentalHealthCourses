using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Articles.Commands.Ctreate;
using MentalHealthcare.Application.Articles.Commands.Delete;
using MentalHealthcare.Application.Articles.Commands.Update;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Articles.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace MentalHealthcare.API.Controllers
{
    [AllowAnonymous]

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]
    //[Route("api/[controller]")]
    //[ApiController]
    public class ArticleController(IMediator mediator
) : ControllerBase
    {

        [HttpPost]
    [SwaggerOperation( 
        Summary = "Create And Insert new Article with it's details [Title - Content - Images - AuthorID]") ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateArticle([FromForm] CreateArticleCommand command)
        {
            var articleId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetArticleById), new { ArticleId = articleId }, null);
        }

    
    [SwaggerOperation(Summary = "Get the add detailed with it's id",
        Description = ArticleControllerDocs.GetByIdDescription
        )]
    [ProducesResponseType(typeof(ArticleDto), 200)]
    [HttpGet("{articleId}")]
        public async Task<IActionResult> GetArticleById([FromRoute] int articleId)
        {
            var query = new GetArticleByIdQuery()
            {
                ArticleID = articleId
            };
            var Article = await mediator.Send(query);
            return Ok(Article);
        }


        [SwaggerOperation(Summary = "Get all Articles",
       Description = ArticleControllerDocs.GetAllDescription)]
        [ProducesResponseType(typeof(PageResult<ArticleDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllArticles([FromQuery] GetAllArticlesQuery query)
        {
            var Articles = await mediator.Send(query);
            return Ok(Articles);
        }


        [SwaggerOperation(
       Summary = "Updated Existing Article",
       Description = ArticleControllerDocs.UpdateDescription)]

        [HttpPut("{ArticletId}")]
        public async Task<IActionResult> UpdateArticle([FromRoute] int ArticletId,
       [FromForm] UpdateArticleCommand command)
        {
            command.ArticleId = ArticletId;
            var Articlet = await mediator.Send(command);
            return CreatedAtAction(nameof(GetArticleById), new { articleId = ArticletId }, null);
        }




        [HttpDelete("{articleId}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int articleId)
        {
            var command = new DeleteArticleCommand()
            {
                Id = articleId
            };
            await mediator.Send(command);
            return NoContent();
        }



    }
}
