using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Course_Interactions.Commands.Complete_Lesson;
using MentalHealthcare.Application.Courses.Course_Interactions.Commands.Enroll_Course;
using MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetLesson;
using MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetMyCourses;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("api/course")]
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CourseInteractionsController(
    IMediator mediator
) : ControllerBase
{
    /// <summary>
    /// Enroll in a course.
    /// </summary>
    [HttpPost("{courseId}/enroll")]
    
    [SwaggerOperation(
        Summary = "Enroll in a Course",
        Description = CourseInteractionDocs.EnrollCourseDescription
    )]
    public async Task<IActionResult> EnrollCourse([FromRoute] int courseId)
    {
        var command = new EnrollCourseCommand
        {
            CourseId = courseId
        };
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Mark a lesson as completed within a course.
    /// </summary>
    [HttpPost("{courseId}/complete/{lessonId}")]
    
    [SwaggerOperation(
        Summary = "Complete a Lesson",
        Description = CourseInteractionDocs.CompleteLessonDescription
    )]
    public async Task<IActionResult> CompleteLesson([FromRoute] int courseId, [FromRoute] int lessonId)
    {
        var command = new CompleteLessonCommand
        {
            CourseId = courseId,
            LessonId = lessonId
        };
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Get the details of a specific lesson within a course.
    /// </summary>
    [HttpGet("{courseId}/watch/{lessonId}")]
    
    [ProducesResponseType(typeof(CourseLessonDto), 200)]
    [SwaggerOperation(
        Summary = "Get Course Lesson",
        Description = CourseInteractionDocs.GetCourseLessonDescription
    )]
    public async Task<IActionResult> GetCourseLesson([FromRoute] int courseId, [FromRoute] int lessonId)
    {
        var query = new GetWatchLessonQuery
        {
            CourseId = courseId,
            LessonId = lessonId
        };
        var result = await mediator.Send(query);
        var op = OperationResult<CourseLessonDto>
            .SuccessResult(result);
        return Ok(op);
    }

    /// <summary>
    /// Get all active courses the user is currently enrolled in along with progress in each course.
    /// </summary>
    [HttpGet("courses/active")]
    
    [SwaggerOperation(
        Summary = "Get Active Courses",
        Description = "Retrieve all the courses the user is currently enrolled in, including progress for each course."
    )]
    public async Task<IActionResult> GetActiveCourses([FromQuery] GetMyCoursesQuery query)
    {
        var result = await mediator.Send(query);
        var op = OperationResult<PageResult<CourseActivityDto>>
            .SuccessResult(result);
        return Ok(op);
    }
}