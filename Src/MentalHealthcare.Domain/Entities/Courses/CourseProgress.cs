namespace MentalHealthcare.Domain.Entities.Courses;

public class CourseProgress
{
    public int CourseProgressId { get; set; }

    public int LastLessonIdx { get; set; } = 0;

    public DateTime LastChange { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
}