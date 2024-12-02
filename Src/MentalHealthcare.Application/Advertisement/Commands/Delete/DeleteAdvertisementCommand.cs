using MediatR;

namespace MentalHealthcare.Application.Advertisement.Commands.Delete;

public class DeleteAdvertisementCommand:IRequest
{
    public int AdvertisementId { get; set; }
}