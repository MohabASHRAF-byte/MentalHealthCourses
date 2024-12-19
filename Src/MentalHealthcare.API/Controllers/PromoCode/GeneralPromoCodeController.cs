using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.PromoCode.General.Commands.AddGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Commands.DeleteGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Commands.UpdateGeneralPromoCode;
using MentalHealthcare.Application.PromoCode.General.Queries.GetGeneralPromoCodeQuery;
using MentalHealthcare.Domain.Dtos.PromoCode;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.PromoCode;

[ApiController]
[Route("api/PromoCode")]
public class GeneralPromoCodeController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(AddGeneralPromoCodeCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { generalPromoCodeId = id }, null);
    }

    [HttpGet("{generalPromoCodeId}")]
    public async Task<ActionResult> Get([FromRoute] int generalPromoCodeId)
    {
        var query = await mediator.Send(new GetGeneralPromoCodeQuery()
        {
            PromoCodeId = generalPromoCodeId
        });
        return Ok(query);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PageResult<GeneralPromoCodeDto>), 200)]
    public async Task<ActionResult> Get(
        [FromQuery] GetGeneralPromoCodeQuery query)
    {
        var queryResult = await mediator.Send(query);
        return Ok(queryResult);
    }

    [HttpDelete("{promoCodeId}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int promoCodeId)
    {
        var command = new DeleteGeneralPromoCodeCommand()
        {
            GeneralPromoCodeId = promoCodeId
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{promoCodeId}")]
    public async Task<IActionResult> Update([FromRoute] int promoCodeId,
        [FromBody] UpdateGeneralPromoCodeCommand updateGeneralPromoCodeCommand)
    {
        updateGeneralPromoCodeCommand.GeneralPromoCodeId = promoCodeId;
        await mediator.Send(updateGeneralPromoCodeCommand);
        return NoContent();
    }
}