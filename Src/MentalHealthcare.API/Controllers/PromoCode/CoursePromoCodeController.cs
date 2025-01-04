using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.Commands.DeleteCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.Commands.UpdateCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.queries.GetAllPromoCodesWithCourseId;
using MentalHealthcare.Application.PromoCode.Course.queries.GetCoursePromoCode;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.PromoCode;

[ApiController]
[Route("api/CoursePromoCode")]
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]

public class CoursePromoCodeController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a course promo code",
        Description = PromoCodesDocs.CreateDescription
    )]
    public async Task<ActionResult> Create(AddCoursePromoCodeCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpGet("{courseId}/promocode/{coursePromoCodeId}")]
    [SwaggerOperation(
        Summary = "Get a specific course promo code",
        Description = "Retrieves a specific promo code by its ID for a course."
    )]
    public async Task<ActionResult> GetCoursePromoCode([FromRoute] int courseId, [FromRoute] int coursePromoCodeId)
    {
        var query = await mediator.Send(new GetCoursePromoCodeQuery()
        {
            CoursePromoCodeId = coursePromoCodeId
        });
        return Ok(query);
    }

    [HttpGet("{courseId}/promocodes")]
    [SwaggerOperation(
        Summary = "Get all promo codes for a course",
        Description = "Retrieves all promo codes associated with a specific course."
    )]
    [ProducesResponseType(typeof(PageResult<CoursePromoCodeDto>), 200)]
    public async Task<ActionResult> GetPromoCodes([FromRoute] int courseId,
        [FromQuery] GetPromoCodeWithCourseIdQuery query)
    {
        query.CourseId = courseId;
        var queryResult = await mediator.Send(query);
        return Ok(queryResult);
    }

    [HttpPut("{courseId}/promocode/{promoCodeId}")]
    [SwaggerOperation(
        Summary = "Update a course promo code",
        Description = PromoCodesDocs.UpdateCoursePromoCodeDescription
    )]
    public async Task<IActionResult> Update([FromRoute] int courseId, [FromRoute] int promoCodeId,
        [FromBody] UpdateCoursePromoCodeCommand updateCoursePromoCodeCommand)
    {
        updateCoursePromoCodeCommand.CoursePromoCodeId = promoCodeId;
        await mediator.Send(updateCoursePromoCodeCommand);
        return NoContent();
    }

    [HttpDelete("{courseId}/promocode/{promoCodeId}")]
    [SwaggerOperation(
        Summary = "Delete a course promo code",
        Description = "Deletes a specific promo code associated with a course."
    )]
    public async Task<IActionResult> Delete([FromRoute] int courseId, [FromRoute] int promoCodeId)
    {
        var command = new DeleteCoursePromoCodeCommand()
        {
            CoursePromoCodeId = promoCodeId
        };
        await mediator.Send(command);
        return NoContent();
    }
}