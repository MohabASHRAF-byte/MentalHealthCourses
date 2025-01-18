using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = Global.DevelopmentVersion)]
public class FileUploadController(
    IConfiguration configuration,
    ILocalizationService localizationService
) : ControllerBase
{
    [HttpGet("message")]
    public async Task<IActionResult> UploadFileAsync(string key, string lang)
    {
        string message = string.Format(
            localizationService.GetMessage("ThumbnailInvalidSize"),
            localizationService.TranslateNumber(7384.898m)
        );
        return Ok(message);
    }
}