using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class PodcastSeriesDto
    {
        public int PodcastSeriesId { get; set; }
        public string NameOfTopic { get; set; } = default!;
        public string? Image { get; set; }
        public string PodcasterName { get; set; } = default!;
        public string? PodcastDescription { get; set; }

        // Avoid circular references by using a simplified Podcast DTO
        public List<PodCastDto>? Podcasts { get; set; }
    }
}
