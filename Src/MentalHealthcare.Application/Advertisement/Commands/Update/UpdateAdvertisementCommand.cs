using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Advertisement.Commands.Update;

public class UpdateAdvertisementCommand:IRequest<int>
{
    public int? AdvertisementId { get; set; }
    public string? AdvertisementName { get; set; }
    public string? AdvertisementDescription { get; set; }
    public bool? IsActive { get; set; }
    public List<IFormFile>? Images { get; set; } = [];
    public List<string>? ImagesUrls { get; set; } = [];
}