using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Reviews.Commands.AddCourseReview;
using MentalHealthcare.Application.Courses.Reviews.Queries.GetAllCourseReviews;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class CourseReviewController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("courses/{courseId}/reviews")]
    public async Task<IActionResult> PostReview([FromRoute] int courseId, AddCourseReviewCommand command)
    {
        command.CourseId = courseId;
        var res = await mediator.Send(command);
        return Ok(res);
    }

    [HttpGet("courses/{courseId}/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<UserReviewDto>), 200)]
    public async Task<IActionResult> GetReview(
        [FromRoute] int courseId,
        [FromQuery] GetAllCourseReviewsQuery query
    )
    {
        query.CourseId = courseId;
        var res = await mediator.Send(query);
        return Ok(res);
    }
}