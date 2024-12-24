namespace MentalHealthcare.Domain.Entities.Courses;

public class FavouriteCourse
{
    public int FavouriteCourseId { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
}