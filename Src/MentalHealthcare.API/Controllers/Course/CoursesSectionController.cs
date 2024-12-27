using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;
using MentalHealthcare.Application.Courses.Sections.Commands.Remove_Section;
using MentalHealthcare.Application.Courses.Sections.Commands.Update_order;
using MentalHealthcare.Application.Courses.Sections.Commands.Update_Section;
using MentalHealthcare.Application.Courses.Sections.Queries.Get_All;
using MentalHealthcare.Application.Courses.Sections.Queries.Get_By_Id;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers.Course;

[ApiController]
[Route("api/courses/{courseId}/sections")]
[ApiExplorerSettings(GroupName = Global.DashboardVersion)]
public class CoursesSectionController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a new section to a course",
        Description = "Adds a new section to the specified course."
    )]
    public async Task<IActionResult> AddNewSection(
        [FromRoute] int courseId,
        AddCourseSectionCommand command)
    {
        command.CourseId = courseId;
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetSectionById), new { courseId, sectionId = result }, result);
    }

    [HttpDelete("{sectionId}")]
    [SwaggerOperation(
        Summary = "Remove a section from a course",
        Description = "Deletes the specified section from the course."
    )]
    public async Task<IActionResult> RemoveSection([FromRoute] int courseId, [FromRoute] int sectionId,
        RemoveCourseSectionCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{sectionId}")]
    [SwaggerOperation(
        Summary = "Update section details in a course",
        Description = "Updates the details of the specified section in the course."
    )]
    public async Task<IActionResult> UpdateSection([FromRoute] int courseId, [FromRoute] int sectionId,
        [FromBody] UpdateSectionCommand command)
    {
        command.CourseId = courseId;
        command.SectionId = sectionId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("order")]
    [SwaggerOperation(
        Summary = "Change the order of sections in a course",
        Description = "Updates the order of sections within the specified course."
    )]
    public async Task<IActionResult> ChangeSectionOrder([FromRoute] int courseId,
        [FromBody] UpdateSectionsOrderCommand command)
    {
        command.CourseId = courseId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{sectionId}")]
    [SwaggerOperation(
        Summary = "Get a section by its ID",
        Description = "Fetches details of a specific section from the course."
    )]
    public async Task<IActionResult> GetSectionById([FromRoute] int courseId, [FromRoute] int sectionId)
    {
        var query = new GetSectionByIdCommand()
        {
            CourseId = courseId,
            SectionId = sectionId
        };

        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all sections in a course",
        Description = "Fetches all sections within the specified course."
    )]
    public async Task<IActionResult> GetSections([FromRoute] int courseId, [FromQuery] GetAllCourseSectionsQuery query)
    {
        query.courseId = courseId;
        var result = await mediator.Send(query);
        return Ok(result);
    }
}