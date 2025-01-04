using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.PromoCode.General.Commands.AddGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Commands.DeleteGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Commands.UpdateGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Queries.GetAllGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Queries.GetGeneralPromoCodeQuery;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.PromoCode;

[ApiController]
[Route("api/promocode")]
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]

public class GeneralPromoCodeController(
    IMediator mediator
) : ControllerBase
{
    /// <summary>
    /// Create a new general promo code.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new promo code.", Description = PromoCodesDocs.CreateDescription)]
    public async Task<ActionResult> Create(AddGeneralPromoCodeCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { generalPromoCodeId = id }, null);
    }

    /// <summary>
    /// Get a specific promo code by ID.
    /// </summary>
    [HttpGet("{generalPromoCodeId}")]
    [SwaggerOperation(Summary = "Retrieve a promo code by ID.")]
    public async Task<ActionResult> Get([FromRoute] int generalPromoCodeId)
    {
        var query = await mediator.Send(new GetGeneralPromoCodeQuery
        {
            PromoCodeId = generalPromoCodeId
        });
        return Ok(query);
    }

    /// <summary>
    /// Get a paginated list of all general promo codes.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PageResult<GeneralPromoCodeDto>), 200)]
    [SwaggerOperation(Summary = "Retrieve all promo codes.", Description = PromoCodesDocs.GetGeneralPromoCodesDescription)]
    public async Task<ActionResult> Get([FromQuery] GetAllGeneralPromoCodeQuery query)
    {
        var queryResult = await mediator.Send(query);
        return Ok(queryResult);
    }

    /// <summary>
    /// Delete a specific promo code by ID.
    /// </summary>
    [HttpDelete("{promoCodeId}")]
    [SwaggerOperation(Summary = "Delete a promo code by ID.")]
    public async Task<IActionResult> Delete([FromRoute] int promoCodeId)
    {
        var command = new DeleteGeneralPromoCodeCommand
        {
            GeneralPromoCodeId = promoCodeId
        };
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Update an existing promo code by ID.
    /// </summary>
    [HttpPut("{promoCodeId}")]
    [SwaggerOperation(Summary = "Update a promo code by ID.")]
    public async Task<IActionResult> Update([FromRoute] int promoCodeId, [FromBody] UpdateGeneralPromoCodeCommand updateGeneralPromoCodeCommand)
    {
        updateGeneralPromoCodeCommand.GeneralPromoCodeId = promoCodeId;
        await mediator.Send(updateGeneralPromoCodeCommand);
        return NoContent();
    }
}
