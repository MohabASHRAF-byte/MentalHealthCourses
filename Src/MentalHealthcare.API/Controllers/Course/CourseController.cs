// using MediatR;
// using MentalHealthcare.Application.Common;
// using MentalHealthcare.Application.Courses.Commands.AddThumbnail;
// using MentalHealthcare.Application.Courses.Commands.Create;
// using MentalHealthcare.Application.Courses.Commands.DeleteThumbnail;
// using MentalHealthcare.Application.Courses.Course.Queries.GetAll;
// using MentalHealthcare.Application.Courses.Queries.GetAll;
// using MentalHealthcare.Application.Courses.Queries.GetById;
// using MentalHealthcare.Domain.Dtos;
// using Microsoft.AspNetCore.Mvc;
//
// namespace MentalHealthcare.API.Controllers.Course;
//
// [ApiController]
// [Route("Api/[controller]")]
// public class CourseController(
//     IMediator mediator
// ) : ControllerBase
// {
//     [HttpPost]
//     public async Task<IActionResult> CreateCourse(CreateCourseCommand command)
//     {
//         var result = await mediator.Send(command);
//         return CreatedAtAction(nameof(GetCourse), new { courseId = result.CourseId }, null);
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCoursesQuery query)
//     {
//         var result = await mediator.Send(query);
//         var ret = OperationResult<PageResult<CourseViewDto>>.SuccessResult(result);
//         return Ok(ret);
//     }
//
//     [HttpGet("{courseId}")]
//     public async Task<ActionResult<Domain.Entities.Course>> GetCourse([FromRoute] int courseId)
//     {
//         var query = new GetCourseByIdQuery { Id = courseId };
//         var result = await mediator.Send(query);
//         return Ok(result);
//     }
//
//     [HttpPost("{courseId}/Thumbnail")]
//     public async Task<IActionResult> UpdateThumbnail([FromForm] AddCourseThumbnailCommand command)
//     {
//         var result = await mediator.Send(command);
//         var ret = OperationResult<string>.SuccessResult(result);
//         return Ok(ret);
//     }
//
//     [HttpDelete("{courseId}/Thumbnail")]
//     public async Task<IActionResult> UpdateThumbnail(int courseId)
//     {
//         var command = new DeleteCourseThumbnailCommand
//         {
//             CourseId = courseId
//         };
//         await mediator.Send(command);
//         return NoContent();
//     }
//
//    
// }