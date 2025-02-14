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
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
public class ContactUsController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create new contact us form",
        Description = ContactUsDocs.SubmitContactUsDescription
    )]
    [ApiExplorerSettings(GroupName = Global.MobileVersion)]
    public async Task<IActionResult> Post(SubmitContactUsCommand command)
    {
        var result = await mediator.Send(command);
        var op = OperationResult<object>
            .SuccessResult(new
            {
                ContactUsId = result
            });
        return Ok(op);
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(
        Summary = "Delete contact messages",
        Description = "Deletes all contact messages with the specified IDs."
    )]
    public async Task<IActionResult> Delete(DeleteContactUsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{formId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(ContactUsForm), 200)]
    [SwaggerOperation(
        Summary = "Get contact form by ID",
        Description = "Retrieves a single contact form by its unique identifier."
    )]
    public async Task<IActionResult> GetFormId([FromRoute] int formId)
    {
        var query = new GetContactFormByIdQuery { Id = formId };
        var result = await mediator.Send(query);
        var op = OperationResult<ContactUsForm>
            .SuccessResult(result);
        return Ok(op);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<ContactUsForm>), 200)]
    [SwaggerOperation(
        Summary = "Get all contact forms",
        Description = ContactUsDocs.GetAllFormsDescription
    )]
    public async Task<IActionResult> GetForms([FromQuery] GetAllContactFormsQuery query)
    {
        var result = await mediator.Send(query);
        var op =  OperationResult<PageResult<ContactUsForm>>
            .SuccessResult(result);
        return Ok(op);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(
        Summary = "Change read state",
        Description = "Updates the read state of a contact form by its ID."
    )]
    public async Task<IActionResult> ReadState(ChangeReadStateCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}