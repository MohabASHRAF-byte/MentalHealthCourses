using MediatR;
using MentalHealthcare.Application.TermsAndConditions.Commands.Create;
using MentalHealthcare.Application.TermsAndConditions.Commands.Delete;
using MentalHealthcare.Application.TermsAndConditions.Commands.Update;
using MentalHealthcare.Application.TermsAndConditions.Queries;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

/// <summary>
/// API controller for managing terms and conditions.
/// </summary>
[ApiController]
[Route("TermsAndConditions")]
public class TermsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TermsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new term or condition.
    /// </summary>
    /// <param name="command">The command containing the term's details.</param>
    /// <response code="204">The term was created successfully.</response>
    /// <response code="400">Invalid data was provided.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateTermCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing term or condition.
    /// </summary>
    /// <param name="command">The command containing the ID of the term to delete.</param>
    /// <response code="204">The term was deleted successfully.</response>
    /// <response code="404">The term was not found.</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(DeleteTermCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Retrieves all terms and conditions.
    /// </summary>
    /// <response code="200">The list of terms and conditions.</response>
    ///
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get All Terms and Conditions",
        Description = "No Need to send any information it will return the result\n" 
                      )]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTerm()
    {
        var query = new GetTermsAndConditionsQuery();
        var terms = await _mediator.Send(query);
        return Ok(terms);
    }

    /// <summary>
    /// Updates an existing term or condition.
    /// </summary>
    [HttpPatch]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [SwaggerOperation(
        Summary = "Update an existing term or condition",
        Description = "To update a term, send an object with the following structure:\n" +
                      @"```json
                        {
                          ""id"": 1,
                          ""name"": ""Updated Term Name OR Existing Term Name"",
                          ""description"": ""Updated description of the term. OR existing term doesn't exist.""
                        }```")]
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateTermCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}