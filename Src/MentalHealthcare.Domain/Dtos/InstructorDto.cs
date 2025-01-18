using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Domain.Dtos;

public class InstructorDto
{
    public int InstructorId { get; set; }
    public string Name { get; set; } = default!;
    public string? About { get; set; }
    public string? ImageUrl { get; set; }
}