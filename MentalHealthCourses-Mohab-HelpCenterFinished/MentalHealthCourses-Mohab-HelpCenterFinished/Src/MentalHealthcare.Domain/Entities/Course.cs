using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentalHealthcare.Domain.Constants;

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
 */public class Course
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
    public string CollectionId { set; get; }=default!;
    public string Description { get; set; } = default!;
    public bool IsFree { get; set; } = false;
    public bool IsPublic { get; set; } = false;

    public List<CourseMateriel> CourseMateriels { set; get; } = new();
    
    [Required] 
    [ForeignKey(nameof(Instructor))]
    public int InstructorId { get; set; } // Foreign Key property
    public Instructor Instructor { get; set; } = default!; // Navigation property

    public ICollection<Category>? Categories { get; set; } = new HashSet<Category>();
    public IEnumerable<SystemUser> UsersFavCourse { get; set; } = new HashSet<SystemUser>();
    public IEnumerable<SystemUser> UsersRates { get; set; } = new HashSet<SystemUser>();
}
