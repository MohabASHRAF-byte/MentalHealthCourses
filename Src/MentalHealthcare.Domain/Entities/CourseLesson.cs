using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;

public class CourseLesson
{
    public int CourseLessonId { get; set; }
    [MaxLength(500)] public string LessonName { get; set; } = string.Empty;
    public int Order { get; set; }
    public string MaterielBunneyId { get; set; } = string.Empty;
    public ContentType ContentType { set; get; }
    public string Url { get; set; } = string.Empty;
    public string LessonBunnyName { get; set; } = string.Empty;
    #region RelationShips

    public List<CourseLessonResource>? CourseLessonResources { get; set; } 
    public int AdminId { get; set; }

    public Admin Admin { get; set; } = default!;
    public int CourseSectionId { get; set; }

    public CourseSection CourseSection { get; set; }

    #endregion
}