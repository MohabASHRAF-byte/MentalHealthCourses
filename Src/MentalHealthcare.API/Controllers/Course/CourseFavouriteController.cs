using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Favourite.Commands.Toggle_favourite;
using MentalHealthcare.Application.Courses.Favourite.Queries.GetFavouriteCourse;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("Api/Favourite")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class CourseFavouriteController(
    IMediator mediator
) : ControllerBase
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

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<CourseViewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFavouriteCourses([FromQuery] GetFavouriteCoursesQuery query)
    {
        var result = await mediator.Send(query);

        return Ok(result);
    }
}