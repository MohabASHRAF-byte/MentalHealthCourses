using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.ContactUs.Commands.Change_Read_state;
using MentalHealthcare.Application.ContactUs.Commands.Create;
using MentalHealthcare.Application.ContactUs.Commands.Delete;
using MentalHealthcare.Application.ContactUs.Queries.GetAll;
using MentalHealthcare.Application.ContactUs.Queries.GetById;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = Global.AllVersion)]

public class ContactUsController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]

    [SwaggerOperation(Summary = "Create new contact us form",
        Description = ContactUsDocs.SubmitContactUsDescription)
    ]
    public async Task<IActionResult> Post(SubmitContactUsCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetFormId), new { formId = result }, null);
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    [SwaggerOperation(Summary = "Delete contact Msgs ",
        Description = "Delete all contact msgs with the passed ids")]

    public async Task<IActionResult> Delete(DeleteContactUsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{formId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    [ProducesResponseType(typeof(ContactUsForm), 200)]

    public async Task<IActionResult> GetFormId([FromRoute] int formId)
    {
        var query = new GetContactFormByIdQuery()
        {
            Id = formId
        };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]

    [ProducesResponseType(typeof(PageResult<ContactUsForm>), 200)]
    [SwaggerOperation(Description = ContactUsDocs.GetAllFormsDescription)]

    public async Task<IActionResult> GetForms([FromQuery] GetAllContactFormsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer")]

    [SwaggerOperation(Description = "Change the state with the passed id to the sent state")]


    public async Task<IActionResult> ReadState(ChangeReadStateCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}