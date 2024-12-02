using MentalHealthcare.Application.BunnyServices;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(
    IConfiguration configuration
) : ControllerBase
{
   
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        var Cdn = new BunnyClient(configuration);
        var response = await Cdn.DeleteFile("marwan2.png");
        return Ok(response);
    }
}