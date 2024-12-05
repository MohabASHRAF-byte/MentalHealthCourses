using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.Delete;

public class DeleteVideoCommand : IRequest<bool>
{
    public string VideoId { get; set; } = default!;
}