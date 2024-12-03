using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;
using MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;
using MentalHealthcare.Application.Courses.Sections.Commands.Update_order;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("Api/{courseId}/Sections")]
public class CoursesSectionController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddNewSection([FromRoute] int courseId, AddCourseSectionCommand command)
    {
        command.CourseId = courseId;
        var result = await mediator.Send(command);
        //todo
        //created
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveSection([FromRoute] int courseId, RemoveCourseSectionCommand command)
    {
        command.CourseId = courseId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch]
    [SwaggerOperation(
        Description = CourseDocs.UpdateSectionOrderDescription
        )]
    public async Task<IActionResult> ChangeSectionOrder([FromRoute] int courseId, [FromBody] UpdateSectionsOrderCommand command)
    {
        command.CourseId = courseId;
        await mediator.Send(command);
        return NoContent();
    }

}