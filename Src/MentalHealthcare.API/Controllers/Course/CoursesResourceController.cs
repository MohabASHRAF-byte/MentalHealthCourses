using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Delete_Resource;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Update_resource_Order;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Update_Resource;
using MentalHealthcare.Application.Courses.LessonResources.Commands.Upload_Resource;
using MentalHealthcare.Application.Courses.LessonResources.Queries.Get_By_id;
using MentalHealthcare.Application.Courses.LessonResources.Queries.GetAll_Resources;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("Api/Courses/{courseId}/Sections/{sectionId}/Lessons/{lessonId}/Resources")]
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]

public class CoursesResourceController(IMediator mediator) : ControllerBase
{
    // 1. Get all resources for the lesson
    [HttpGet]
    public async Task<IActionResult> GetLessonResources(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId)
    {
        var query = new GetLessonResourceByLessonIdQuery
        {
            CourseId = courseId,
            SectionId = sectionId,
            LessonId = lessonId,
        };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    // 2. Get a specific resource by its ID
    [HttpGet("{resourceId}")]
    public async Task<IActionResult> GetLessonResourceById(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromRoute] int resourceId)
    {
        var query = new GetResourceByIdQuery
        {
            CourseId = courseId,
            SectionId = sectionId,
            LessonId = lessonId,
            ResourceId = resourceId
        };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    // 3. Upload a new resource to the lesson
    [HttpPost]
    [SwaggerOperation(
        Description = CourseResourceDocs.UploadLessonResourceDescription
        )]
    public async Task<IActionResult> UploadLessonResource(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromForm] UploadLessonResourceCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // 4. Update an existing resource
    [HttpPatch("{resourceId}")]
    public async Task<IActionResult> UpdateLessonResource(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromRoute] int resourceId,
        [FromForm] UpdateLessonResourceCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        command.ResourceId = resourceId;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // 5. Update the order of resources
    [HttpPatch("Order")]
    public async Task<IActionResult> UpdateLessonResourceOrder(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromBody] UpdateResourceOrderCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // 6. Delete an existing resource from the lesson
    [HttpDelete("{resourceId}")]
    public async Task<IActionResult> DeleteLessonResource(
        [FromRoute] int courseId,
        [FromRoute] int sectionId,
        [FromRoute] int lessonId,
        [FromRoute] int resourceId,
        [FromForm] DeleteLessonResourceCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        command.LessonId = lessonId;
        command.ResourceId = resourceId;
        await mediator.Send(command);
        return NoContent();
    }
}
