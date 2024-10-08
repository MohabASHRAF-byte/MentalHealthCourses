using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.CreateVideo;

public class CreateVideoCommand : IRequest<string?>
{
    public string LibraryId { get; set; } = default!;
    public string CollectionId { get; set; } =default!;
    public string VideoName { get; set; } = default!;
}