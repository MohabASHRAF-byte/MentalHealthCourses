using MentalHealthcare.Domain.Dtos.course;
using System.Collections.Generic;

namespace MentalHealthcare.Domain.Dtos
{
    public class InstructorDto
    {
        public string Name { get; set; } 
        public string? ImageUrl { get; set; }
        public string? About { get; set; } 
        public int InstructorId { get; set; } 

        public List<CourseViewDto>? Courses { get; set; } // Nullable in case no courses are assigned yet
    }
}
