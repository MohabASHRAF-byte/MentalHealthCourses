using MediatR;
using MentalHealthcare.Application.OrderProcessing.Order.Commands.Place;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.OrderProcessing;

[ApiController]
[Route("api/[controller]")]
public class OrderController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> post([FromBody] PlaceOrderCommand command)
    {
        var res = await mediator.Send(command);
        return Ok(res);
    }
}