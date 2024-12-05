using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class ArticleDto
    {

        public int ArticleId { get; set; }
        public string Title { get; set; } = default!; 
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string PhotoUrl { get; set; } = default!; 
        public DateTime CreatedDate { get; set; }



    }
}
