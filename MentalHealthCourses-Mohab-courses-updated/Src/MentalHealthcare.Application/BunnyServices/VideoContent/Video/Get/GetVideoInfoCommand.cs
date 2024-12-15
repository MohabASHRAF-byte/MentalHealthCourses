using MediatR;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.Get;

public class GetVideoInfoCommand:IRequest<VideoInfoDto?>
{
    public string LibraryId { get; set; } = default!;
    public string VideoId { get; set; } = default!;
}
