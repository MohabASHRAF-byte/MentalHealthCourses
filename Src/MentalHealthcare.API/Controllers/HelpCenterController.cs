using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.HelpCenterItem.Commands.Create;
using MentalHealthcare.Application.HelpCenterItem.Commands.Delete;
using MentalHealthcare.Application.HelpCenterItem.Commands.Update;
using MentalHealthcare.Application.HelpCenterItem.Queries;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("api/helpCenter")]
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]
public class HelpCenterController(
    IMediator mediator
) : ControllerBase
{
    /// <summary>
    /// Creates a new Help Center item.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create Help Center Item",
            Description = HelpCenterControllerDocs.HelpCenterPostDescription
        )
    ]
    public async Task<IActionResult> Post(CreateHelpCenterItemCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing Help Center item.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Delete Help Center Item",
        Description = "Deletes an existing item in the Help Center by ID."
    )]
    public async Task<IActionResult> Delete(DeleteHelpCenterItemCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Retrieves Help Center items based on the specified query.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Help Center Items",
        Description = HelpCenterControllerDocs.GetAllSummery
    )]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

    public async Task<IActionResult> GetTerm([FromQuery] GetHelpCenterItemQuery query)
    {
        var terms = await mediator.Send(query);
        var op = OperationResult<List<HelpCenterItem>>
            .SuccessResult(terms);
        return Ok(op);
    }

    /// <summary>
    /// Updates an existing Help Center item.
    /// </summary>
    [HttpPut]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Update Help Center Item",
        Description = HelpCenterControllerDocs.HelpCenterUpdateDescription
    )]
    public async Task<IActionResult> Update(UpdateHelpCenterCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}