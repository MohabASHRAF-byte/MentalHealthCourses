using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Advertisement.Queries.GetAll;

public class GetAllAdvertisementsQuery : IRequest<PageResult<AdvertisementDto>>
{
    public int IsActive { get; set; } = 3;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}