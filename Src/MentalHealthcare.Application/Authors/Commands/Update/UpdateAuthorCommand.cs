using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Commands.Update
{
   public class UpdateAuthorCommand : IRequest<int>
    { public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorAbout { get; set; }
        public  IFormFile ImagesUrl { get; set; } }
}
