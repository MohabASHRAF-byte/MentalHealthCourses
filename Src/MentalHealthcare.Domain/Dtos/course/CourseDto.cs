using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

// Written By Marcelino , Reviewed by Mohab
// Reviewed
/* Review
   =========
    - Remove title from course
   - changel thumbnail to stringUrl
   - add isFree bool 
   - add reviewsCount
   - add is public
   - change rating to nullable decimal instead of int
 */public class CourseDto
{
    [Key]
    public int CourseId { set; get; }
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailName { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public int EnrollmentsCount { get; set; } = 0;
    // public string CollectionId { set; get; }=default!;
    public string Description { get; set; } = default!;
    public bool IsFree { get; set; } = false;
    public bool IsPublic { get; set; } = false;
    [Required] 
    [ForeignKey(nameof(Instructor))]
    public InstructorDto Instructor { get; set; } = default!; // Navigation property
    public List<CourseSectionDto> CourseSections { set; get; } = new List<CourseSectionDto>();
    public ICollection<CategoryDto>? Categories { get; set; } = new HashSet<CategoryDto>();
    public IEnumerable<SystemUser> UsersFavCourse { get; set; } = new HashSet<SystemUser>();
    public IEnumerable<SystemUser> UsersRates { get; set; } = new HashSet<SystemUser>();
}
