using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Delete
{
    public class DeletePodcastSeriesCommand : IRequest
    {
        public int SeriesId { get; set; }
    }
}
