using MediatR;

namespace MentalHealthcare.Application.BunnyServices.Files.DeleteFile;

public class DeleteFileCommand:IRequest
{
    public string Directory { get; set; } =default!;
    public string FileName { get; set; } =default!;
}