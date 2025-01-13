using MentalHealthcare.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos
{
    public class ArticleDto
    {

        public int ArticleId { get; set; }
        public List<string> ArticleImageUrls { get; set; } = [];
        [MaxLength(Global.ArticleTitleMaxLength)]
        public string Title { get; set; }
        [MaxLength(Global.ArticleContent)]
        public string Content { get; set; }

        public string AuthorName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;



    }
}
