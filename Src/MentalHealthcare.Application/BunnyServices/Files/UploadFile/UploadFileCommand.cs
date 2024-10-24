using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.BunnyServices.UploadFile;

public class UploadFileCommand : IRequest<string>
{
    public IFormFile File { get; set; } = default!;
    public string Directory { get; set; } = default!;
    public string FileName { get; set; } = default!;
}