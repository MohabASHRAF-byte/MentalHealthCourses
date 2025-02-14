using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Favourite.Commands.Toggle_favourite;
using MentalHealthcare.Application.Courses.Favourite.Queries.GetFavouriteCourse;
using MentalHealthcare.Application.Courses.Favourite.Queries.GetUsersWhoFavouriteCourse;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("api/Favourite")]
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
public class CourseFavouriteController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Toggle Favourite Course",
        Description = CourseFavouriteDocs.ToggleFavouriteCourseDescription)]
    public async Task<IActionResult> ToggleFavouriteCourse([FromRoute] int courseId)
    {
        var command = new ToggleFavouriteCourseCommand
        {
            CourseId = courseId,
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<SystemUser>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Users Who Favourited Course",
        Description = CourseFavouriteDocs.GetUsersWhoFavouriteCourseDescription)]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]
    public async Task<IActionResult> GetUsersWhoFavouriteCourse([FromRoute] int courseId)
    {
        var command = new GetUsersWhoFavouriteCourseQuery
        {
            CourseId = courseId,
        };
        var result = await mediator.Send(command);
        var op = OperationResult<PageResult<SystemUser>>.SuccessResult(result);
        return Ok(op);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(PageResult<CourseViewDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Favourite Courses",
        Description = CourseFavouriteDocs.GetFavouriteCoursesDescription)]
    public async Task<IActionResult> GetFavouriteCourses([FromQuery] GetFavouriteCoursesQuery query)
    {
        var result = await mediator.Send(query);
        var op = OperationResult<PageResult<CourseViewDto>>.SuccessResult(result);
        return Ok(op);
    }
}