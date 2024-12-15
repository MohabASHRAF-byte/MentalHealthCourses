using MediatR;
using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommand : IRequest<int>
    {



        public int ArticleId { get; set; }
        [Required]
        public string Content { get; set; } = default!;
        [Required]
        public string Title { get; set; } = default!;

        public List<IFormFile>? Image_Article { get; set; } = [];
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Author? Author { get; set; } = default!;







    }
}
