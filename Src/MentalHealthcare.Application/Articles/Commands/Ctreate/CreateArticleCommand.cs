using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Ctreate
{
    public class CreateArticleCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<IFormFile> Images { get; set; } = [];
        public int? AuthorId { get; set; }

    }
}
