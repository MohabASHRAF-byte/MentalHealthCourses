using MediatR;
using MentalHealthcare.Application.Courses.Reviews.Commands.AddCourseReview;
using MentalHealthcare.Domain.Constants;
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

    public async Task<IActionResult> PostReview(AddCourseReviewCommand command)
    {
        var res = await mediator.Send(command);
        return Ok(res);
    }
}