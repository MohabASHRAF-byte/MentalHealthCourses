using MediatR;
using MentalHealthcare.Application.Courses.Lessons.Commands;
using MentalHealthcare.Application.Courses.Lessons.Commands.Add_Lesson;
using MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;
using MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;
using MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
public class CoursesLessonController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [Route("Api/{courseId}/{sectionId}/Lesson/")]
    public async Task<IActionResult> PostNewLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromBody] AddLessonCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        var result = await mediator.Send(command);
        //todo 
        // create at action
        return Ok(result);
    }

    [HttpDelete]
    [Route("Api/{courseId}/{sectionId}/Lesson/")]
    public async Task<IActionResult> DeleteLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromBody] RemoveLessonCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut]
    [Route("Api/{courseId}/{sectionId}/{lessonId}/")]
    public async Task<IActionResult> UpdateLessonData(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromBody] UpdateLessonCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch]
    [Route("Api/{courseId}/{sectionId}/")]
    public async Task<IActionResult> UpdateLessonOrder(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromBody] UpdateLessonsOrderCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        await mediator.Send(command);
        return NoContent();
    }
}