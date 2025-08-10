using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Commands.Update
{
    public class UpdateEpisodesCommand : IRequest<int>
    {
        public int PodcastId { get; set; }
        public int EpisodeLength { get; set; }
        public IFormFile AudioFile { get; set; } = default!;
        public int PodcastSeriesId { get; set; }
        public string Title { get; set; } = default!;




    }
}
