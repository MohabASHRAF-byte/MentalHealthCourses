namespace MentalHealthcare.Domain.Dtos;

public class CourseViewDto
{
   public string Name { set; get; } = default!;
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public int EnrollmentsCount { get; set; } = 0;
}