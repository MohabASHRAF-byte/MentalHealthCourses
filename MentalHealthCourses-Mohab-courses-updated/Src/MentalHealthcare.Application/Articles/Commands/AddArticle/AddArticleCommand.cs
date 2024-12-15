using MediatR;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.AddArticle
{
    public class AddArticleCommand : IRequest<int>
    {
        [Required] public string Title { get; set; } = default!;
        [Required] public string Content { get; set; } = default!;
        [Required] public int AuthorId { get; set; } // Foreign Key property for the Author
        [Required] public int UploadedById { get; set; } // Foreign Key property for the Admin who uploads
        public List<IFormFile> Image_Article { get; set; } = new();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string UploadedBy { get; set; } = default!;
        public string Author { get; set; } = default!;



    }
}
