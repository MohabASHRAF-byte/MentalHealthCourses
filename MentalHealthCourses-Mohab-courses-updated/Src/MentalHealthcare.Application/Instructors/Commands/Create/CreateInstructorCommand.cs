using MediatR;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Command.Create
{
    public class CreateInstructorCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public IFormFile ImageUrl { get; set; }
        public string? About { get; set; }
        public string AddedBy { get; set; } = default!;
        public ICollection<CourseDto>? Courses { get; set; }

    }
}
