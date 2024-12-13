using MediatR;
using MentalHealthcare.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Command.Update
{
    public class UpdateInstructorCommand : IRequest<int>
    { public int InstructorId { get; set; }
        public string Name { get; set; } = default!;
        public IFormFile ImageUrl { get; set; }
        public string? About { get; set; }
        public string AddedBy { get; set; } = default!;
        public ICollection<string>? Courses { get; set; }
    }
}