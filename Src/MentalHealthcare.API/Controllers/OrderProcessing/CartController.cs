using MediatR;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Add_to_Cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Delete_from_cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.OrderProcessing;

[ApiController]
[Route("api/[controller]")]
public class CartController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> Post(AddToCartCommand command)
    {
        var id = await mediator.Send(command);
        //todo: craeted at
        return Created();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute] int itemId)
    {
        var command = new DeleteFromCartCommand()
        {
            CourseId = itemId
        };
        await mediator.Send(command);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetCartItemsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllFromCart()
    {
        await mediator.Send(new DeleteFromCartCommand());
        return Ok();
    }
}