using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Courses;

public class CourseViewDto
{
   public string Name { set; get; } = default!;
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public int EnrollmentsCount { get; set; } = 0;
}