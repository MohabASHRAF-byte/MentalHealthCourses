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
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class OrderController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    [ApiExplorerSettings(GroupName = Global.MobileVersion)]

    public async Task<IActionResult> post([FromBody] PlaceOrderCommand command)
    {
        var res = await mediator.Send(command);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    [ProducesResponseType(typeof(PageResult<InvoiceDto>), 200)]
    [SwaggerOperation(Description = OrderProcessingDocs.GetAllInvoicesDescription)]
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

    public async Task<IActionResult> GetAllOrders([FromQuery] GetAllInvoicesQuery query)
    {
        var res = await mediator.Send(query);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{invoiceId}")]
    [SwaggerOperation(Description = OrderProcessingDocs.GetInvoiceDescription)]
    [ApiExplorerSettings(GroupName = Global.SharedVersion)]

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
    [HttpPost("CalculateInvoiceValue")]
    [SwaggerOperation(Description = OrderProcessingDocs.CalculateInvoiceDescription)]
    [ProducesResponseType(typeof(CalculateInvoiceResponse), 200)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    public async Task<IActionResult> CalculateInvoice([FromBody] CalculateInvoice command)
    {
        var res = await mediator.Send(command);
        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("{invoiceId}/accept")]
    [SwaggerOperation(Description = OrderProcessingDocs.AcceptInvoiceDescription)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    public async Task<IActionResult> AcceptInvoice([FromRoute] int invoiceId,
        [FromBody] AcceptInvoiceCommand command
    )
    {
        command.InvoiceId = invoiceId;
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("{invoiceId}/status")]
    [SwaggerOperation(Description = OrderProcessingDocs.ChangeInvoiceStateDescription)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

    public async Task<IActionResult> ChangeInvoiceStatus([FromRoute] int invoiceId)
    {
        var command = new ChangeInvoiceStateCommand()
        {
            InvoiceId = invoiceId
        };
        await mediator.Send(command);
        return NoContent();
    }
}