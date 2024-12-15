using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

// written By Mohab , No Reviews Yet
namespace MentalHealthcare.Domain.Entities;

public class CourseLessonResource
{
    public int CourseLessonResourceId { get; set; }


    [MaxLength(Global.TitleMaxLength)]
    public string? Title { set; get; }

    public int ItemOrder { get; set; }

    [MaxLength(Global.UrlMaxLength)]
    public string Url { set; get; } = default!;

    public string BunnyId { set; get; } = default!;
    public string BunnyPath { set; get; } = default!;
    public ContentType ContentType { set; get; } = default!;

    #region RelationShips

    public int CourseLessonId { get; set; }
    public CourseLesson CourseLesson { get; set; } = default!;

    #endregion
}