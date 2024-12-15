using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class MeditationDto
    {



        public string Title { get; set; } = default!;

        public int UploadedById { get; set; } // Foreign Key property
        public string UploadedBy { get; set; } = default!;

        public DateTime CreatedDate { get; set; }

        public int MeditationId { get; set; }

        //todo implement it with url to avoid heavy db searches
        public string Content { get; set; } = default!;






    }
}
