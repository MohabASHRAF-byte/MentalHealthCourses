using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.HomeScreenItems.Queries.GetAdvertisements;
using MentalHealthcare.Application.HomeScreenItems.Queries.GetRecentlyAddedCourses;
using MentalHealthcare.Application.HomeScreenItems.Queries.GetTrainers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/homescreen")] 
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
public class HomeScreenController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(Summary = "Get all Advertisements",
           Description = HomeScreenControllerDocs.GetAllAdvertisements)]
    [ProducesResponseType(typeof(PageResult<AdvertisementDto>), 200)]
    [HttpGet("advertisements")] 
    public async Task<IActionResult> GetAdvertisements([FromQuery] GetAdvertisementsQuery query)
    {
        var advertisements = await mediator.Send(query);
        return Ok(advertisements);
    }
    [SwaggerOperation(Summary = "Get Most Recent Courses",
               Description = HomeScreenControllerDocs.MostRecentCoursesSection)]
    [ProducesResponseType(typeof(PageResult<CourseViewDto>), 200)]
    [HttpGet("recent-courses")] // ✅ Unique route
    public async Task<IActionResult> GetRecentlyAddedCourses([FromQuery] GetRecentlyAddedCoursesQuery query)
    {
        var courses = await mediator.Send(query);
        return Ok(courses);
    }
    [SwaggerOperation(Summary = "Get all Trainers",
           Description = HomeScreenControllerDocs.GetAllInstructorDescription)]
    [ProducesResponseType(typeof(PageResult<InstructorDto>), 200)] // ✅ Fixed response type
    [HttpGet("trainers")] // ✅ Unique route
    public async Task<IActionResult> GetTrainers([FromQuery] GetTrainersQuery query)
    {
        var trainers = await mediator.Send(query);
        return Ok(trainers);
    }
}
