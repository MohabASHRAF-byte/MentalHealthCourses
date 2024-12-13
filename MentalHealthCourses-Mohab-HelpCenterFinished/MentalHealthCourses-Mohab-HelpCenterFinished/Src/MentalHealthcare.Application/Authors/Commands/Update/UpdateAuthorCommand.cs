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
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = default!;
        public IFormFile ImageUrl { get; set; }
        public string? About { get; set; }
        public string AddedBy { get; set; } = default!;
        public List<string> Articles { get; set; } = new();
    }
}
