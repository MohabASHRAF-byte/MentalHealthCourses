using MediatR;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.BunnyServices.PodCast.Get;

public class GetPodCastQuery : IRequest<PodCastDto>
{
    public string PodCastId { set; get; }=default!;
}