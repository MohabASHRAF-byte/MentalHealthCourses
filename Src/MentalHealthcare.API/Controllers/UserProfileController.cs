using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Application.SystemUsers.Profile.GetUserProfileData;
using MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = Global.AllVersion)]
public class UserProfileController(
    IMediator mediator
) : ControllerBase
{
    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(Description = UserProfileDocs.UpdateProfileDescription)]
    public async Task<IActionResult> UpdateProfile(
        UpdateProfileDataCommand command
    )
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [SwaggerOperation(Description = UserProfileDocs.GetProfileDescription)]
    public async Task<IActionResult> GetProfile()
    {
        var query = new GetUserProfileDataQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
}