using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Podcast : MaterialBE
    {

        public int PodcastId { get; set; }


        public PodCaster podCasters { get; set; }

    }
}
