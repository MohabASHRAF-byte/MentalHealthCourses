using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Dtos.course;

public class CourseActivityDto
{
    public int CourseId { set; get; }
    [MaxLength(Global.TitleMaxLength)] public string Name { set; get; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    public int WatchedLessons { set; get; }
    public int AllLessons { set; get; }
    public decimal CompletionPercentage { set; get; }
}