using MediatR;
using MentalHealthcare.Application.Author.Commands.Create;
using MentalHealthcare.Application.Author.Queries.GetById;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Application.Instructors.Queries.GetById;
using MentalHealthcare.Domain;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{[AllowAnonymous]

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]
    public class AuthorController ( IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Author", 
            Description = "Creates new Author with it's details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthor([FromForm] CreateAuthorCommand command)
        {
            var AuthorId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetAuthorById), new { AuthorId = AuthorId }, null);
        }  

        
        
        
        [SwaggerOperation(Summary = "Get the Author detailed with it's id")]
        [ProducesResponseType(typeof(AuthorDto), 200)]

        [HttpGet("{AuthorId}")]
        public async Task<IActionResult> GetAuthorById([FromRoute] int AuthorId)
        {
            var query = new GetAuthorByIdQuery()
            {
               AuthorID  = AuthorId
            };
            var authorid = await mediator.Send(query);
            return Ok(AuthorId);
        }
        
        
        
        
        
        
        
        
        
    }
}
