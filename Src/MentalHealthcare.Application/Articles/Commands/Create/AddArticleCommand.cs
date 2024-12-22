using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Create
{
    public class AddArticleCommand : IRequest<int>
    {

        [Required] public string Title { get; set; } = default!;
        [Required] public string Content { get; set; } = default!;
        [Required] public int AuthorId { get; set; } // Foreign Key property for the Author
        [Required] public int UploadedById { get; set; } // Foreign Key property for the Admin who uploads
        public required IFormFile Image_Article { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string UploadedBy { get; set; } = default!;
        public string Author { get; set; } = default!;





    }
}
