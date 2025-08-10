using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Commands.Delete
{
    public class DeleteEpisodeCommand : IRequest
    {

        public int EpisodeId { get; set; }




    }
}
