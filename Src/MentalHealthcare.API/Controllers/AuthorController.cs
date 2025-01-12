using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Application.Authors.Commands.Create;
using MentalHealthcare.Application.Authors.Commands.Delete;
using MentalHealthcare.Application.Authors.Commands.Update;
using MentalHealthcare.Application.Authors.Queries.GetAll;
using MentalHealthcare.Application.Authors.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace MentalHealthcare.API.Controllers;
    [AllowAnonymous]

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]


    public class AuthorController (IMediator mediator) : ControllerBase
    {

    [HttpPost]
    [SwaggerOperation(
           Summary = "Creates new Author",
           Description = "Creates new Author with it's details"
       )
   ]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAuthor([FromForm] CreateAuthorCommand command)
    {
        var authorId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAuthorById), new { authorId = authorId }, null);
    }



    [SwaggerOperation(Summary = "Get the Author detailed with it's id")]
    [ProducesResponseType(typeof(AuthorDto), 200)]

    [HttpGet("{AuthorId}")]
    public async Task<IActionResult> GetAuthorById([FromRoute] int AuthorId)
    {
        var query = new GetAuthorByIdQuery()
        {
            AuthorId = AuthorId
        };
        var author = await mediator.Send(query);
        return Ok(author);
    }


    [SwaggerOperation(Summary = "Get all Authors")]
    [ProducesResponseType(typeof(PageResult<AuthorDto>), 200)]
    [HttpGet]
    public async Task<IActionResult> GetAllAuthors([FromQuery] GetAllAuthorsQuery query)
    {
        var Authors = await mediator.Send(query);
        return Ok(Authors);
    }



    [HttpPut("{AuthorId}")]
    [SwaggerOperation(Summary = "Updated Existing Author")]
    public async Task<IActionResult> UpdateAuthor([FromRoute] int authorId, [FromForm] UpdateAuthorCommand command)
    {
        command.AuthorId = authorId;
        var updatedAuthorId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAuthorById), new { authorId = updatedAuthorId }, null);
    }



    [HttpDelete("{authorId}")]
    public async Task<IActionResult> DeleteAuthor([FromRoute] int authorId)
    {
        var command = new DeleteAuthorCommand
        {
            AuthorID = authorId
        };
        await mediator.Send(command);
        return NoContent();
    }






}

