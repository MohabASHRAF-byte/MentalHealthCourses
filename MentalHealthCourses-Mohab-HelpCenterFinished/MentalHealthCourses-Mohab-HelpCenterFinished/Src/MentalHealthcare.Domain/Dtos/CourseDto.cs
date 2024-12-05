using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos;

public class CourseDto
{
    public int CourseId { set; get; }
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public int EnrollmentsCount { get; set; } = 0;
    public string Description { get; set; } = default!;
    public bool IsFree { get; set; } = false;
    public bool IsPublic { get; set; } = false;
    public int favouritesCount { get; set; } = 0;
    public List<CourseMaterielDto> CourseMateriels { set; get; } = new();
    public InstructorDto Instructor { get; set; } = default!; // Navigation property

    public ICollection<CategoryDto>? Categories { get; set; }
    // public IEnumerable<SystemUser> UsersRates { get; set; } = new HashSet<SystemUser>();
}