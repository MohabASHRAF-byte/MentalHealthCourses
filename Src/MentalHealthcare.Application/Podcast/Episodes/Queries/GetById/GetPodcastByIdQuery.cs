using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Queries.GetById
{
    public class GetPodcastByIdQuery : IRequest<PodCastDto>
    { public int PodCastid { get; set; }}
}
