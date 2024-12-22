using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos.OrderProcessing;

public class CourseCartDto
{
    [MaxLength(Global.TitleMaxLength)] 
    public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)]
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    
}