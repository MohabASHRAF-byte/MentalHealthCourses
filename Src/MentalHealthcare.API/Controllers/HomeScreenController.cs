using MediatR;
using MentalHealthcare.API.Docs;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.HomeScreenItems.Queries.GetHomeScreenItems;

using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentalHealthcare.API.Controllers;
[ApiController]
[Route("api/homescreen")]
[ApiExplorerSettings(GroupName = Global.MobileVersion)]
[Authorize(AuthenticationSchemes = "Bearer")]
public class HomeScreenController(IMediator mediator) : ControllerBase
{



    [SwaggerOperation(
           Summary = "Get Home Screen Items")]
    [ProducesResponseType(typeof(HomeScreenDto), 200)]
    [HttpGet] 
    public async Task<IActionResult> GetHomeScreenItems([FromQuery] GetHomeScreenItemsQuery query)
    {
        var homeScreenData = await mediator.Send(query);
        var op = OperationResult<PageResult<HomeScreenDto>>
                    .SuccessResult(homeScreenData);
        return Ok(op);
    }









}
