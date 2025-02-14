using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Add_to_Cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Clear_Cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Commands.Delete_from_cart;
using MentalHealthcare.Application.OrderProcessing.Cart.Queries.GetCartItems;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.OrderProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.OrderProcessing;

[ApiController]
[Route("api/cart")]
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CartController(
    IMediator mediator
) : ControllerBase
{
    /// <summary>
    /// Add an item to the cart.
    /// </summary>
    [HttpPost("items")]
    [SwaggerOperation(Summary = "Add to Cart", Description = OrderProcessingDocs.AddToCartDescription)]
    public async Task<IActionResult> AddToCart(AddToCartCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Retrieve all items in the cart.
    /// </summary>
    [HttpGet("items")]
    [ProducesResponseType(typeof(CartDto), 200)]
    [SwaggerOperation(Summary = "Get Cart Items", Description = OrderProcessingDocs.GetCartItemsDescription)]
    public async Task<IActionResult> GetCartItems([FromQuery] GetCartItemsQuery query)
    {
        var result = await mediator.Send(query);
        var op = OperationResult<CartDto>
            .SuccessResult(result);
        return Ok(op);
    }

    /// <summary>
    /// Remove a specific item from the cart.
    /// </summary>
    [HttpDelete("items/{itemId}")]
    [SwaggerOperation(Summary = "Remove from Cart", Description = OrderProcessingDocs.DeleteFromCartDescription)]
    public async Task<IActionResult> RemoveFromCart([FromRoute] int itemId)
    {
        var command = new DeleteFromCartCommand
        {
            CourseId = itemId
        };
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Clear all items from the cart.
    /// </summary>
    [HttpDelete]
    [SwaggerOperation(Summary = "Clear Cart", Description = OrderProcessingDocs.ClearCartDescription)]
    public async Task<IActionResult> ClearCart()
    {
        await mediator.Send(new ClearCartItemsCommand());
        return NoContent();
    }
}