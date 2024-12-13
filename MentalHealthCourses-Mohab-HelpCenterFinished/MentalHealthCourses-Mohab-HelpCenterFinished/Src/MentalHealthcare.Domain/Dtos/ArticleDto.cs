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
     public string Title { get; set; } = default!;
     public string Content { get; set; } = default!;
     public string PhotoUrl { get; set; } = default!;
     public DateTime CreatedDate { get; set; }
     public int UploadedById { get; set; }
    public int AuthorId { get; set; } 
     public Admin UploadedBy { get; set; } = default!;
     public AuthorDto Author { get; set; } = default!;



    }
}
