using MediatR;
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

[AllowAnonymous] // This already allows anonymous access, so no need for additional [Authorize]
[ApiController]
[Route("api/homescreen")] // Define a base route for better structure
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
public class HomeScreenController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all Advertisements.
    /// </summary>
    [SwaggerOperation(Summary = "Get all Advertisements")]
    [ProducesResponseType(typeof(PageResult<AdvertisementDto>), 200)]
    [HttpGet("advertisements")] // ✅ Unique route
    public async Task<IActionResult> GetAdvertisements([FromQuery] GetAdvertisementsQuery query)
    {
        var advertisements = await mediator.Send(query);
        return Ok(advertisements);
    }

    /// <summary>
    /// Get recently added courses.
    /// </summary>
    [SwaggerOperation(Summary = "Get Recently Added Courses")]
    [ProducesResponseType(typeof(PageResult<CourseViewDto>), 200)]
    [HttpGet("recent-courses")] // ✅ Unique route
    public async Task<IActionResult> GetRecentlyAddedCourses([FromQuery] GetRecentlyAddedCoursesQuery query)
    {
        var courses = await mediator.Send(query);
        return Ok(courses);
    }

    /// <summary>
    /// Get all trainers.
    /// </summary>
    [SwaggerOperation(Summary = "Get all Trainers")]
    [ProducesResponseType(typeof(PageResult<InstructorDto>), 200)] // ✅ Fixed response type
    [HttpGet("trainers")] // ✅ Unique route
    public async Task<IActionResult> GetTrainers([FromQuery] GetTrainersQuery query)
    {
        var trainers = await mediator.Send(query);
        return Ok(trainers);
    }
}
