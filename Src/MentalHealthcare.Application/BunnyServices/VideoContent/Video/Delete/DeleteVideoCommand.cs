using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.Delete;

public class DeleteVideoCommand : IRequest<bool>
{
    public string LibraryId { get; set; } = default!;
    public string VideoId { get; set; } = default!;
}