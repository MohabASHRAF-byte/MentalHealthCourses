using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application
{
    public class ArticlesDto
    {


        public int ArticleId { get; set; }
        public int AuthorId { get; set; }
        public int UploadedById { get; set; } // Foreign Key property
        public string Content { get; set; } = default!;
        public string PhotoUrl { get; set; } = default!;
        public string Title { get; set; } = default!;
        public DateTime CreatedDate { get; set; }

        public string UploadedBy { get; set; } = default!;
        public string AuthorinDto { get; set; } = default!;










    }
}
