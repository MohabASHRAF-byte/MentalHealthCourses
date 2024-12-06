using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.BunnyServices.VideoContent.Video;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(
    IConfiguration configuration
) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, string filename, string extension = ".mp3")
    {
        var Cdn = new BunnyClient(configuration);
        var response = await Cdn.UploadFileAsync(file, filename + extension, "Podcast");
        return Ok(response);
    }
}