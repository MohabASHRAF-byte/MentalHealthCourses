using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Update
{
    public class UpdateArticleCommand : IRequest<int>
    {

        public int? ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? Images { get; set; } = [];
        public List<string>? ImagesUrls { get; set; } = [];

        public int? AuthorId { get; set; }












    }
}
