using MediatR;
using MentalHealthcare.Application.Certificates.Commands.Generate;
using MentalHealthcare.Application.Certificates.Commands.Send;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers
{
    [AllowAnonymous]

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]
    public class CertificateController(IMediator mediator) : ControllerBase
    {


        // Endpoint to generate a certificate
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCertificate([FromBody] GenerateCertificateCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Execute the command to generate the certificate
                var result = await mediator.Send(command);
                return Ok(result); // Returning the path of the generated certificate or other result
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to send the certificate via email
        [HttpPost("send")]
        public async Task<IActionResult> SendCertificate([FromBody] SendCertificateCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Execute the command to send the certificate
                await mediator.Send(command);
                return Ok("Certificate sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






    }
    }
