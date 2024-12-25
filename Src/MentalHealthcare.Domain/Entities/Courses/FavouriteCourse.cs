namespace MentalHealthcare.Domain.Entities.Courses;

public class FavouriteCourse
{
    public int FavouriteCourseId { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int SystemUserId { get; set; }
    public SystemUser SystemUser { get; set; }
}