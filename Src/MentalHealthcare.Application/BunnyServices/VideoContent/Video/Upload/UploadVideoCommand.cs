using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.Upload;

public class UploadVideoCommand : IRequest<bool>
{
    public string VideoId { get; set; } = default!;
    public string LibraryName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
}