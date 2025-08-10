using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Update
{
    public class UpdatePodcastSeriesCommand : IRequest <int>
    {
        public int PodcastSeriesId { get; set; }
        public IFormFile? Image { get; set; }
        public string NameOfTopic { get; set; } = default!;
        public string? PodcastDescription { get; set; }
        public string PodcasterName { get; set; } = default!;
      //  public int UploadedById { get; set; } // Ensure this field exists


    }
}
