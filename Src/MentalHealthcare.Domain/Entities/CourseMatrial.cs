namespace MentalHealthcare.Domain.Entities;

public class CourseMateriel
{
    public int CourseMaterielId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public int ItemOrder { get; set; }
    public string Url { set; get; } = default!;
    public bool IsConfirmed { set; get; }
    public bool IsVideo { set; get; }
    public string? Description { set; get; }
    public string? Title { set; get; }
}