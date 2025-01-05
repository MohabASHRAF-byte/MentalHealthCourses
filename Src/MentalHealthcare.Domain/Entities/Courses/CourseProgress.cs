namespace MentalHealthcare.Domain.Entities.Courses;

public class CourseProgress
{
    public int CourseProgressId { get; set; }

    public int LastLessonIdx { get; set; } = 0;

    public DateTime LastChange { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int SystemUserId { get; set; }
    public SystemUser SystemUser { get; set; }
}