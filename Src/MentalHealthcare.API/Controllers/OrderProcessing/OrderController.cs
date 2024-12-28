using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.OrderProcessing.Order.Commands.Accept_Invoice;
using MentalHealthcare.Application.OrderProcessing.Order.Commands.Calculate_value;
using MentalHealthcare.Application.OrderProcessing.Order.Commands.Change_State;
using MentalHealthcare.Application.OrderProcessing.Order.Commands.Place;
using MentalHealthcare.Application.OrderProcessing.Order.Queries.Get_All_Invoices;
using MentalHealthcare.Application.OrderProcessing.Order.Queries.Get_invoice;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.OrderProcessing;

[ApiController]
[Route("api/orders")]
[ApiExplorerSettings(GroupName = Global.AllVersion)]
public class OrderController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    [SwaggerOperation(Summary = "Place a new order.", Description = "Creates a new order for the authenticated user.")]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        var res = await mediator.Send(command);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    [ProducesResponseType(typeof(PageResult<InvoiceDto>), 200)]
    [SwaggerOperation(Summary = "Retrieve all orders.", Description = OrderProcessingDocs.GetAllInvoicesDescription)]
    public async Task<IActionResult> GetAllOrders([FromQuery] GetAllInvoicesQuery query)
    {
        var res = await mediator.Send(query);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{invoiceId}")]
    [SwaggerOperation(Summary = "Retrieve a specific invoice.", Description = OrderProcessingDocs.GetInvoiceDescription)]
    public async Task<IActionResult> GetInvoice([FromRoute] int invoiceId)
    {
        var query = new GetInvoiceQuery()
        {
            InvoiceId = invoiceId
        };
        var res = await mediator.Send(query);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("calculate-value")]
    [SwaggerOperation(Summary = "Calculate invoice value.", Description = OrderProcessingDocs.CalculateInvoiceDescription)]
    [ProducesResponseType(typeof(CalculateInvoiceResponse), 200)]
    public async Task<IActionResult> CalculateInvoice([FromBody] CalculateInvoice command)
    {
        var res = await mediator.Send(command);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("{invoiceId}/accept")]
    [SwaggerOperation(Summary = "Accept an invoice.", Description = OrderProcessingDocs.AcceptInvoiceDescription)]
    public async Task<IActionResult> AcceptInvoice([FromRoute] int invoiceId,
        [FromBody] AcceptInvoiceCommand command
    )
    {
        command.InvoiceId = invoiceId;
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPatch("{invoiceId}/state")]
    [SwaggerOperation(Summary = "Change invoice state.", Description = OrderProcessingDocs.ChangeInvoiceStateDescription)]
    public async Task<IActionResult> ChangeInvoiceStatus([FromRoute] int invoiceId, [FromBody] ChangeInvoiceStateCommand command)
    {
        command.InvoiceId = invoiceId;
        await mediator.Send(command);
        return NoContent();
    }
}
