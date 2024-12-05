using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
// written By Mohab , No Reviews Yet
namespace MentalHealthcare.Domain.Entities;

public class CourseMateriel
{
    public int CourseMaterielId { get; set; }
    public string MaterielBunneyId { get; set; }=string.Empty;

    public int AdminId { get; set; }

    public Admin Admin { get; set; } = default!;
    //
    public int CourseId { get; set; }

    public Course Course { get; set; } = default!;

    //
    //
    public int CourseSectionId { get; set; }
    public CourseSection CourseSection { get; set; } = default!;
    //
    public int CourseLessonId { get; set; }
    public CourseLesson CourseLesson { get; set; } = default!;
    //
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