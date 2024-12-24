using MediatR;
using MentalHealthcare.Application.Courses.Favourite.Commands.Add_favourite.Add_Course_Favourite;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("Api/Favourite")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]

public class CourseFavouriteController(
    IMediator mediator
    ): ControllerBase
{

    [HttpPost("{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ToggleFavouriteCourse([FromRoute] int courseId)
    {
        var command = new ToggleFavouriteCourseCommand()
        {
            CourseId = courseId,
        };
        await mediator.Send(command);
        return NoContent();
    }
}