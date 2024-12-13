using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MentalHealthcare.Domain.Dtos;

public class InstructorDto
{   public int InstructorId { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? About { get; set; }
    public Admin AddedBy { get; set; } = default!;
    public ICollection<CourseDto>? Courses { get; set; }
}