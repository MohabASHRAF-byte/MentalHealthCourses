using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

// written By Mohab , No Reviews Yet
namespace MentalHealthcare.Domain.Dtos.course;

public class CourseResourceDto
{
    public int CourseLessonResourceId { get; set; }
    
    [MaxLength(Global.TitleMaxLength)] 
    public string? Title { set; get; }
    public int ItemOrder { get; set; }

    [MaxLength(Global.UrlMaxLength)] public string Url { set; get; } = default!;
    
    public ContentType ContentType { set; get; } = default!;
    //
}