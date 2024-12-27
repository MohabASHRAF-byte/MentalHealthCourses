using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Courses.Course_Interactions.Commands.Complete_Lesson;
using MentalHealthcare.Application.Courses.Course_Interactions.Commands.Enroll_Course;
using MentalHealthcare.Application.Courses.Course_Interactions.Queries.GetLesson;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class CourseInteractionsController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("Enroll/{courseId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    // [SwaggerOperation(Description = CourseInteractionDocs.EnrollCourseDescription)]
    public async Task<IActionResult> EnrollCourse([FromRoute] int courseId)
    {
        var command = new EnrollCourseCommand
        {
            CourseId = courseId
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{courseId}/complete/{lessonId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerOperation(Description = CourseInteractionDocs.CompleteLessonDescription)]
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

    [HttpGet("{courseId}/watch/{lessonId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(CourseLessonDto), 200)]
    [SwaggerOperation(Description = CourseInteractionDocs.GetCourseLessonDescription)]
    public async Task<IActionResult> GetCourseLesson([FromRoute] int courseId, [FromRoute] int lessonId)
    {
        var query = new GetWatchLessonQuery()
        {
            CourseId = courseId,
            LessonId = lessonId
        };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}