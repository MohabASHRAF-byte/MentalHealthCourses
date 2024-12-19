using MediatR;
using MentalHealthcare.Application.PromoCode.Course.Commands.AddCoursePromoCode;
using MentalHealthcare.Application.PromoCode.Course.queries.GetCoursePromoCode;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.PromoCode;

[ApiController]
[Route("api/CoursePromoCode")]
public class CoursePromoCodeController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(AddCoursePromoCodeCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { coursePromoCodeId = id }, null);
    }

    [HttpGet("{coursePromoCodeId}")]
    public async Task<ActionResult> Get([FromRoute] int coursePromoCodeId)
    {
        var query = await mediator.Send(new GetCoursePromoCodeQuery()
        {
            CoursePromoCodeId = coursePromoCodeId
        });
        return Ok(query);
    }
}