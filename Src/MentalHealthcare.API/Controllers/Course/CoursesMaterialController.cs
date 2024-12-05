using MediatR;
using MentalHealthcare.Application.Courses.Materials.Commands.ConfirmUpload;
using MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;
using MentalHealthcare.Application.Courses.Materials.Commands.Upload_pdf;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("Api/{courseId}/{sectionId}/{lessonId}/")]
public class CoursesMaterialController(
    IMediator mediator
) : ControllerBase
{
    // [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpPost("Video")]
    public async Task<IActionResult> AddVideo(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        CreateVideoCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        var result = await mediator.Send(command);
        return Ok(result);
    }
    // [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpPost("ConfirmVideo")]
    public async Task<IActionResult> ConfirmVideo([FromRoute] int courseId, ConfirmUploadCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("Pdf")]
    public async Task<IActionResult> AddPdf(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromForm] UploadPdfCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        await mediator.Send(command);
        return NoContent();
    }
}