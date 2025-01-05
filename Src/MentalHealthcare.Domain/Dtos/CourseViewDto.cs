using MentalHealthcare.Domain.Dtos.Category;

namespace MentalHealthcare.Domain.Dtos;

public class CourseViewDto
{
    public int CourseId { get; set; }
    public string Name { set; get; } = default!;
    public string? ThumbnailUrl { get; set; }
    public string? IconUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public int EnrollmentsCount { get; set; } = 0;
    public bool IsOwned { get; set; } = false;
    public bool IsFree { get; set; } = false;
    public List<MiniCategoryDto> Categories { get; set; } = [];
}