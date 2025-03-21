using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class MeditationDto
    {public string Title { get; set; }

        //todo implement it with url to avoid heavy db searches
        public string Content { get; set; } = default!;

        public string? Image { get; set; }
    
    
    
    
    }
}
