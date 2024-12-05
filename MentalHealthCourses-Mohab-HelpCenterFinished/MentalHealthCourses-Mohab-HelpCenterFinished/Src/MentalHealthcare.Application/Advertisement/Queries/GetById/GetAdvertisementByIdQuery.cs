using MediatR;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Advertisement.Queries.GetById;

public class GetAdvertisementByIdQuery: IRequest<AdvertisementDto>
{
    public int AdvertisementId { get; set; }
}