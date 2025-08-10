using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetById
{
    public class GetPodcastSeriesByIdQuery : IRequest<PodcastSeriesDto>
    {public int PodcastSeriesId { get; set; }


    }
}
