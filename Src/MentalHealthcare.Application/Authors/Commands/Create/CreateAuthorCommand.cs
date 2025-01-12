using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Commands.Create
{
    public class CreateAuthorCommand : IRequest<int>
    {



        public string Name { get; set; }
        public string? About { get; set; }
        public IFormFile? ImageUrl { get; set; }



    }
}
