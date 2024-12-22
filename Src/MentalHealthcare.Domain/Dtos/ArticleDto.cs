using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
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

        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        [MaxLength(Global.UrlMaxLength)]
        public string PhotoUrl { get; set; } = default!;
        public int? UploadedById { get; set; } // Foreign Key property

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int AuthorId { get; set; } // Foreign Key property

        public Author Author { get; set; } = default!;
        public Admin? UploadedBy { get; set; } = default!;








    }
}
