using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcasts.Queries.GetById
{
    public class GetByIdPodcastsQuery : IRequest<PodCastDto>
    {

        public int PodcastId { get; set; }

    }
}
