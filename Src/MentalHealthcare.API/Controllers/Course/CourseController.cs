using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Course.Commands.AddIcon;
using MentalHealthcare.Application.Courses.Course.Commands.AddThumbnail;
using MentalHealthcare.Application.Courses.Course.Commands.Create;
using MentalHealthcare.Application.Courses.Course.Commands.DeleteIconCommand;
using MentalHealthcare.Application.Courses.Course.Commands.DeleteThumbnail;
using MentalHealthcare.Application.Courses.Course.Commands.UpdateCourseCommand;
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
    /// <summary>
    /// Create a new course.
    /// </summary>
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Create a new course", Description = CourseDocs.CreateCourseDescription)]
    public async Task<IActionResult> CreateCourse(
        [FromBody] CreateCourseCommand command
    )
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCourseById), new { courseId = result.CourseId }, null);
    }

    /// <summary>
    /// Get all courses with optional filters.
    /// </summary>
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Get all courses", Description = CourseDocs.GetAllCoursesDescription)]
    [ProducesResponseType(typeof(OperationResult<PageResult<CourseViewDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCoursesQuery query)
    {
        var result = await mediator.Send(query);
        var operationResult = OperationResult<PageResult<CourseViewDto>>.SuccessResult(result);
        return Ok(operationResult);
    }

    /// <summary>
    /// Get a specific course by its ID.
    /// </summary>
    [HttpGet("{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get a course by ID", Description = CourseDocs.GetCourseByIdDescription)]
    public async Task<ActionResult<Domain.Entities.Courses.Course>> GetCourseById([FromRoute] int courseId)
    {
        var query = new GetCourseByIdQuery { Id = courseId };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing course.
    /// </summary>
    [HttpPut("{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Update a course", Description = CourseDocs.UpdateCourseDescription)]
    public async Task<IActionResult> UpdateCourse(
        [FromRoute] int courseId,
        [FromBody] UpdateCourseCommand command
    )
    {
        command.CourseId = courseId;
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Add or update the course thumbnail.
    /// </summary>
    [HttpPost("{courseId}/thumbnail")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Add or update course thumbnail", Description = CourseDocs.AddOrUpdateThumbnailDescription)]
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

    /// <summary>
    /// Delete the course thumbnail.
    /// </summary>
    [HttpDelete("{courseId}/thumbnail")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Delete course thumbnail")]
    public async Task<IActionResult> DeleteThumbnail([FromRoute] int courseId)
    {
        var command = new DeleteCourseThumbnailCommand
        {
            CourseId = courseId
        };
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Add a course icon.
    /// </summary>
    [HttpPost("{courseId}/icon")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Add a course icon", Description = CourseDocs.AddCourseIconDescription)]
    public async Task<IActionResult> AddCourseIcon(
        [FromRoute] int courseId,
        [FromForm] AddCourseIconCommand command
    )
    {
        command.CourseId = courseId;
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete the course icon.
    /// </summary>
    [HttpDelete("{courseId}/icon")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Summary = "Delete course icon")]
    public async Task<IActionResult> DeleteCourseIcon(
        [FromRoute] int courseId,
        [FromForm] DeleteCourseIconCommand command
    )
    {
        command.CourseId = courseId;
        await mediator.Send(command);
        return NoContent();
    }
}
