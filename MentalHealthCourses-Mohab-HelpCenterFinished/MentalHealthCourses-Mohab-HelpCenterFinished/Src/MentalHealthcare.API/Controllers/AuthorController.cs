using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Authors.Commands.Create;
using MentalHealthcare.Application.Authors.Commands.Delete;
using MentalHealthcare.Application.Authors.Commands.Update;
using MentalHealthcare.Application.Authors.Queries.GetAll;
using MentalHealthcare.Application.Authors.Queries.GeyById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {private readonly IMediator _mediator;
    public AuthorController(IMediator mediator){ _mediator = mediator;}
        [HttpPost]
        [SwaggerOperation(Summary = "Creates new Author", Description = "Creates new Author with its details")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthor([FromForm] CreateAuthorCommand command)
        {var AuthorId = await _mediator.Send(command);
         return CreatedAtAction(nameof(GetAuthorById), new { AuthorId }, null);}

        [HttpGet("{AuthorId}")]
        [SwaggerOperation(Summary = "Get the Author by its ID")]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthorById([FromRoute] int AuthorId)
        {var query = new GetAuthorByIdQuery { AuthorId = AuthorId };
            var Author = await _mediator.Send(query);
            return Ok(Author);}

        [HttpGet]
        [SwaggerOperation(Summary = "Get All Authors")]
        [ProducesResponseType(typeof(PageResult<AuthorDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAuthors([FromQuery] GetAllAuthorsQuery query)
        {var Authors = await _mediator.Send(query);
            return Ok(Authors);}

        [HttpPut("{authorId}")]
        [SwaggerOperation(Summary = "Update Existing Author")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateAuthor([FromRoute] int authorId, [FromForm] UpdateAuthorCommand command)
        {command.AuthorId = authorId;
            var Author = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAuthorById), new { AuthorId = authorId }, null);}



        [HttpDelete("{authorId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int authorId)
        {
            var command = new DeleteAuthorCommand { AuthorId = authorId };
            await _mediator.Send(command);
            return NoContent();
        }


    }
}
