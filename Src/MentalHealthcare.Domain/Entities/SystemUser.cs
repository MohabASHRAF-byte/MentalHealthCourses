namespace MentalHealthcare.Domain.Entities;

public class SystemUser:HumanBe
{
    public int SystemUserId { get; set; }
    public DateTime Dof { get; set; }
    public List<Course>? FavCourses { get; set; } = new();

    public List<Course>? CourseRates { get; set; } = new();

    public List<Logs>? Logs { get; set; } = new();
    public List<Payments>? Payments { get; set; } = new();
    //
    public User User { get; set; }
}
