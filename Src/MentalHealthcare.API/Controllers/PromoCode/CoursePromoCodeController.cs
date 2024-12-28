using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.PromoCode.Course;
using MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.Commands.DeleteCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.Commands.UpdateCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.queries.GetAllPromoCodesWithCourseId;
using MentalHealthcare.Application.PromoCode.Course.queries.GetCoursePromoCode;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.PromoCode;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.PromoCode;

[ApiController]
[Route("api/CoursePromoCode")]
[ApiExplorerSettings(GroupName = Global.AllVersion)]

public class CoursePromoCodeController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(AddCoursePromoCodeCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpGet("course/{courseId}/promocode{coursePromoCodeId}")]
    public async Task<ActionResult> GetCoursePromoCode([FromRoute] int coursePromoCodeId)
    {
        var query = await mediator.Send(new GetCoursePromoCodeQuery()
        {
            CoursePromoCodeId = coursePromoCodeId
        });
        return Ok(query);
    }

    [HttpGet("course/{courseId}/")]
    [SwaggerOperation(Description = CoursePromoCodeDocs.GetPromoCodesWithCourseIdDescription)]
    [ProducesResponseType(typeof(PageResult<CoursePromoCodeDto>), 200)]
    public async Task<ActionResult> Get([FromRoute] int courseId, [FromQuery] GetPromoCodeWithCourseIdQuery query)
    {
        query.CourseId = courseId;
        var queryResult = await mediator.Send(query);
        return Ok(queryResult);
    }

    [HttpDelete("course/{courseId}/promocode/{promoCodeId}")]
    public async Task<IActionResult> Delete([FromRoute] int courseId, [FromRoute] int promoCodeId)
    {
        var command = new DeleteCoursePromoCodeCommand()
        {
            CoursePromoCodeId = promoCodeId
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("course/{courseId}/promocode/{promoCodeId}")]
    public async Task<IActionResult> update([FromRoute] int courseId, [FromRoute] int promoCodeId,
        [FromBody] UpdateCoursePromoCodeCommand updateCoursePromoCodeCommand)
    {
        updateCoursePromoCodeCommand.CoursePromoCodeId = promoCodeId;
        var updateResult = await mediator.Send(updateCoursePromoCodeCommand);
        return NoContent();
    }
}