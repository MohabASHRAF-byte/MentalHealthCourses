using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
   public class MeditationDto 
    {


        public int MeditationId { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int UploadedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UploadedByInDto { get; set; } = default!;








    }
}
