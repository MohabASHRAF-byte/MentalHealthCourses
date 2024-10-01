namespace MentalHealthcare.Domain.Entities;

public class Course
{
    public int CourseId { set; get; }
    public string Name { set; get; }=default!;
    public List<CourseMateriel> CourseMateriels { set; get; } = new();
}