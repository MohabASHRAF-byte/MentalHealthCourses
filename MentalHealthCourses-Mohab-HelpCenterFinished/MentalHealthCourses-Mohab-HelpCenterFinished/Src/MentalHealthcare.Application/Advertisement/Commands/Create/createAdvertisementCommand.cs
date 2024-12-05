using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Advertisement.Commands.Create;

public class CreateAdvertisementCommand : IRequest<int>
{
    public string AdvertisementName { get; set; } = string.Empty;
    public string AdvertisementDescription { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<IFormFile> Images { get; set; } = [];
}