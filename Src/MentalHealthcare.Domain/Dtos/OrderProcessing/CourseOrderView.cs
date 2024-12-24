using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Dtos.OrderProcessing;

public class CourseOrderView
{
    public int CourseOrderViewId { get; set; }
    public int CourseId { get; set; }
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
}