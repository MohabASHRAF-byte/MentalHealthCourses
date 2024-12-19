// using MediatR;
// using MentalHealthcare.Application.Courses.Lessons.Commands.ConfirmUpload;
// using MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;
// using MentalHealthcare.Application.Courses.Lessons.Commands.RemoveLesson;
// using MentalHealthcare.Application.Courses.Lessons.Commands.Update_Lesson;
// using MentalHealthcare.Application.Courses.Lessons.Commands.Update_order;
// using MentalHealthcare.Application.Courses.Lessons.Commands.Upload_pdf;
// using MentalHealthcare.Application.Courses.Lessons.Queries.GetById;
// using MentalHealthcare.Application.Courses.Lessons.Queries.GetLessonsBySectionId;
// using MentalHealthcare.Application.Videos.Commands.CreateVideo;
// using MentalHealthcare.Domain.Dtos.course;
// using Microsoft.AspNetCore.Mvc;
//
// namespace MentalHealthcare.API.Controllers.Course;
//
// [ApiController]
// [Route("api/courses/{courseId}/sections/{sectionId}/lessons")]
// public class LessonsController(IMediator mediator) : ControllerBase
// {
//     [HttpPost("video")]
//     [ProducesResponseType(typeof(CreateVideoCommandResponse), 200)]
//
//     public async Task<IActionResult> AddVideoLesson(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromBody] CreateVideoCommand command)
//     {
//         command.CourseId = courseId;
//         command.CourseSectionId = sectionId;
//         var result = await mediator.Send(command);
//         return Ok(result);
//     }
//
//     [HttpPost("video/confirm")]
//     public async Task<IActionResult> ConfirmVideoLesson(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromBody] ConfirmUploadCommand command)
//     {
//         command.CourseId = courseId;
//         command.SectionId = sectionId;
//         var result = await mediator.Send(command);
//         // await mediator.Send(command);
//         // return CreatedAtAction(nameof(GetAdvertisementById), new { advertisementId = advertisement }, null);
//
//         return Ok(result);
//     }
//
//     [HttpPost("pdf")]
//     public async Task<IActionResult> AddPdfLesson(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromForm] UploadPdfLessonCommand command)
//     {
//         command.CourseId = courseId;
//         command.SectionId = sectionId;
//         var result = await mediator.Send(command);
//         //TODO: createdAtAction
//         return Ok(result);
//     }
//
//     [HttpPatch("{lessonId}")]
//     public async Task<IActionResult> UpdateLesson(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromRoute] int lessonId,
//         [FromBody] UpdateLessonCommand command)
//     {
//         command.CourseId = courseId;
//         command.SectionId = sectionId;
//         command.LessonId = lessonId;
//         var result = await mediator.Send(command);
//         return Ok(result);
//     }
//
//     [HttpPatch("order")]
//     public async Task<IActionResult> UpdateLessonOrder(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromBody] UpdateLessonsOrderCommand command)
//     {
//         command.CourseId = courseId;
//         command.SectionId = sectionId;
//         await mediator.Send(command);
//         return NoContent();
//     }
//
//     [HttpDelete("{lessonId}")]
//     public async Task<IActionResult> RemoveLesson(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromRoute] int lessonId)
//     {
//         var command = new RemoveLessonCommand
//         {
//             CourseId = courseId,
//             SectionId = sectionId,
//             LessonId = lessonId
//         };
//         await mediator.Send(command);
//         return NoContent();
//     }
//     [HttpGet]
//     [ProducesResponseType(typeof(List<CourseLessonDto>), 200)]
//
//     public async Task<IActionResult> GetLessons(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId)
//     {
//         var command = new GetLessonsBySectionIdQuery()
//         {
//             CourseId = courseId,
//             CourseSectionId = sectionId,
//         };
//         var result =await mediator.Send(command);
//         return Ok(result);
//     }
//     [HttpGet("{lessonId}")]
//     [ProducesResponseType(typeof(CourseLessonViewDto), 200)]
//
//     public async Task<IActionResult> GetLesson(
//         [FromRoute] int courseId,
//         [FromRoute] int sectionId,
//         [FromRoute] int lessonId)
//     {
//         var command = new GetLessonByIdQuery()
//         {
//             CourseId = courseId,
//             CourseSectionId = sectionId,
//             LessonId = lessonId
//         };
//         var result =await mediator.Send(command);
//         return Ok(result);
//     }
// }