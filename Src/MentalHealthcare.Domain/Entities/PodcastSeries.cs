using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
   public class PodcastSeries
    {

        public int PodcastSeriesId { get; set; } 

        public string? Image { get; set; }

        [Required]
        [MaxLength(255)]
        public string NameOfTopic { get; set; } = default!; 
        public string? PodcastDescription { get; set; } 

        public string PodcasterName { get; set; }

        public List<Podcast>? Podcasts { get; set; }

    }
}
