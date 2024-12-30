using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseDto
{
    [Key] public int CourseId { set; get; }
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public long SecondsSinceCreation { get; set; }
    public int ReviewsCount { get; set; } = 0;

    public int EnrollmentsCount { get; set; } = 0;

    public string Description { get; set; } = default!;
    public bool IsFree { get; set; } = false;
    [JsonIgnore]
    public bool IsArchived { get; set; } = false;
    public bool? IsFavourite { get; set; } = false;
    public bool? IsOwned { get; set; } = false;

    [Required]
    [ForeignKey(nameof(Instructor))]
    public InstructorDto Instructor { get; set; } = default!; // Navigation property

    public List<CourseSectionDto> CourseSections { set; get; } = new List<CourseSectionDto>();
    public ICollection<CategoryDto>? Categories { get; set; } = new HashSet<CategoryDto>();
    public IEnumerable<UserReviewDto> UserReviews { get; set; } =  [];
}