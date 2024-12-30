using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Course.Commands.AddThumbnail;
using MentalHealthcare.Application.Courses.Course.Commands.Create;
using MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;
using MentalHealthcare.Application.Courses.Course.Queries.GetAll;
using MentalHealthcare.Application.Courses.Course.Queries.GetById;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("api/courses")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class CourseController(IMediator mediator) : ControllerBase
{
    // Create a new course
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Description = CourseDocs.CreateCourseDescription)]
    public async Task<IActionResult> CreateCourse(
        [FromBody] CreateCourseCommand command
    )
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCourseById), new { courseId = result.CourseId }, null);
    }

    // Get all courses
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Description = CourseDocs.GetAllCoursesDescription)]
    [ProducesResponseType(typeof(IEnumerable<CourseViewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCoursesQuery query)
    {
        var result = await mediator.Send(query);
        var operationResult = OperationResult<PageResult<CourseViewDto>>.SuccessResult(result);
        return Ok(operationResult);
    }

    // Get a course by ID
    [HttpGet("{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
     [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<Domain.Entities.Course>> GetCourseById([FromRoute] int courseId)
    {
        var query = new GetCourseByIdQuery { Id = courseId };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    // Add or update course thumbnail
    [HttpPost("{courseId}/thumbnail")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public async Task<IActionResult> AddOrUpdateThumbnail(
        [FromRoute] int courseId,
        [FromForm] AddCourseThumbnailCommand command
        )
    {
        command.CourseId = courseId;
        var result = await mediator.Send(command);
        var operationResult = OperationResult<string>.SuccessResult(result);
        return Ok(operationResult);
    }

    // Delete course thumbnail
    [HttpDelete("{courseId}/thumbnail")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public async Task<IActionResult> DeleteThumbnail([FromRoute] int courseId)
    {
        var command = new DeleteCourseThumbnailCommand
        {
            CourseId = courseId
        };
        await mediator.Send(command);
        return NoContent();
    }
}