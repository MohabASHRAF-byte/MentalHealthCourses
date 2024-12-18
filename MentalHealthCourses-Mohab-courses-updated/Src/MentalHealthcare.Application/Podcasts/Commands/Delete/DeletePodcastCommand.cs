using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcasts.Commands.Delete
{
    public class DeletePodcastCommand : IRequest
    {
        public int podcastId { get; set; }

    }
}
