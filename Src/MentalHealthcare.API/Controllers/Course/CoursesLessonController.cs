using MediatR;
using MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;
using MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;
using MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;
using MentalHealthcare.Application.Courses.Lessons.Commands.Upload_pdf;
using MentalHealthcare.Application.Courses.Materials.Commands.ConfirmUpload;
using MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("Api/{courseId}/{sectionId}/Lesson/")]

public class CoursesLessonController(
    IMediator mediator
) : ControllerBase
{   
    // [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpPost("VideoLesson")]
    public async Task<IActionResult> AddVideoLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        CreateVideoCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        var result = await mediator.Send(command);
        return Ok(result);
    }
    [HttpPost("ConfirmVideoLesson")]
    public async Task<IActionResult> ConfirmVideoLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        CreateVideoCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        var result = await mediator.Send(command);
        return Ok(result);
    }
    [HttpPost("AddPdfLesson")]
    public async Task<IActionResult> ConfirmVideoLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        UploadPdfLessonCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        var result = await mediator.Send(command);
        return Ok(result);
    }
    [HttpPatch("Lesson")]
    public async Task<IActionResult> UpdateLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        UpdateLessonCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        var result = await mediator.Send(command);
        return Ok(result);
    }
    [HttpPatch("UpdateOrder")]
    public async Task<IActionResult> UpdateLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        UpdateLessonsOrderCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        await mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{lessonId}")]
    public async Task<IActionResult> UpdateLesson(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId
        )
    {
        var command = new RemoveLessonCommand();
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        await mediator.Send(command);
        return NoContent();
    }


}