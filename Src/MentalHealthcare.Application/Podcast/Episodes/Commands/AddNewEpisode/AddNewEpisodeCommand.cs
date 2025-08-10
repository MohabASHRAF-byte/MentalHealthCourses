using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Commands.AddNewEpisode
{
    public class AddNewEpisodeCommand : IRequest<int>
    {
        public string Title { get; set; } = default!;
        public IFormFile AudioFileUpload { get; set; } = default!;
        public int PodcastSeriesId { get; set; }

        public int EpisodeLength { set; get; }

    }
}
