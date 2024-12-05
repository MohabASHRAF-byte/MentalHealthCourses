using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

// written By Mohab , No Reviews Yet
namespace MentalHealthcare.Domain.Dtos.course;

public class CourseMaterielDto
{
    public int CourseMaterielId { get; set; }
    
    [MaxLength(Global.TitleMaxLength)] 
    public string? Title { set; get; }

    //todo add max length to description 
    public string? Description { set; get; }

    //
    public int ItemOrder { get; set; }

    [MaxLength(Global.UrlMaxLength)] public string Url { set; get; } = default!;

    

    public bool IsVideo { set; get; }
    //
}