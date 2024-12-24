using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Add_to_Cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Delete_from_cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.OrderProcessing;

[ApiController]
[Route("api/[controller]")]
public class CartController(
    IMediator mediator
) : ControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    [SwaggerOperation(Description = CartEndpointDescriptions.AddToCartDescription)]
    public async Task<IActionResult> Post(AddToCartCommand command)
    {
        var id = await mediator.Send(command);
        return Created($"/api/cart/{id}", new { id });
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("items/{itemId}")]
    [SwaggerOperation(Description = CartEndpointDescriptions.DeleteFromCartDescription)]
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
    [ProducesResponseType(typeof(CartDto), 200)]
    [SwaggerOperation(Description = CartEndpointDescriptions.GetCartItemsDescription)]
    public async Task<IActionResult> GetAll([FromQuery] GetCartItemsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    [SwaggerOperation(Description = CartEndpointDescriptions.ClearCartDescription)]
    public async Task<IActionResult> DeleteAllFromCart()
    {
        await mediator.Send(new ClearCartItemsCommand());
        return Ok();
    }
}

