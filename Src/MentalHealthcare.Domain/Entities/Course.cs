using MentalHealthcare.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace MentalHealthcare.Domain.Entities;

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
 */
public class Course
{
    public int CourseId { set; get; }
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public string Description { get; set; } = default!;
    public bool IsFree { get; set; } = false;
    public bool IsPublic { get; set; } = false;

    public List<CourseMateriel> CourseMateriels { set; get; } = new();

    [Required]
    public int InstructorId { get; set; } // Foreign Key property
    public Instructor Instructor { get; set; } = default!; // Navigation property

    public ICollection<Category>? Categories { get; set; } = new HashSet<Category>();
    public IEnumerable<SystemUser> UsersFavCourse { get; set; } = new HashSet<SystemUser>();
    public IEnumerable<SystemUser> UsersRates { get; set; } = new HashSet<SystemUser>();
}
