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
    {public string Name { get; set; } = default!;
        public IFormFile ImageUrl { get; set; }
        public string? About { get; set; }
        public string AddedBy { get; set; } = default!;}}

        // Name of the  Admin adding the author







