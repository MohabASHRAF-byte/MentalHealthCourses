using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.HelpCenterItem.Commands.Create;
using MentalHealthcare.Application.HelpCenterItem.Commands.Delete;
using MentalHealthcare.Application.HelpCenterItem.Commands.Update;
using MentalHealthcare.Application.HelpCenterItem.Queries;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;


[ApiController]
[Route("HelpCenter")]
[Authorize(AuthenticationSchemes = "Bearer")]

public class HelpCenterController(
    IMediator mediator
) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Description = HelpCenterControllerDocs.HelpCenterPostDescription)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    public async Task<IActionResult> Post(CreateHelpCenterItemCommand command)
    {
        await mediator.Send(command);
        //todo 
        // createdataction
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    public async Task<IActionResult> Delete(DeleteHelpCenterItemCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }


    
    [Produces("application/json")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Description = HelpCenterControllerDocs.HelpCenterGetDescription
    )]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

    public async Task<IActionResult> GetTerm([FromQuery]GetHelpCenterItemQuery query)
    {
        var terms = await mediator.Send(query);
        return Ok(terms);
    }


    [SwaggerOperation(
        Summary = HelpCenterControllerDocs.PatchSummery,
        Description = HelpCenterControllerDocs.HelpCenterUpdateDescription)]
    [HttpPut]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    public async Task<IActionResult> Update(UpdateHelpCenterCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}