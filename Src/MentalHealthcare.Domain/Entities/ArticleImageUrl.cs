using MentalHealthcare.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class ArticleImageUrl
    {
        public int ArticleImageUrlId { get; set; }

        [MaxLength(Global.UrlMaxLength)] // Limit URL length
        public string ImageUrl { get; set; }
        public int ArticleId { get; set; } // Foreign key to Advertisement
        public Article Article { get; set; } // Navigation property









    }
}
