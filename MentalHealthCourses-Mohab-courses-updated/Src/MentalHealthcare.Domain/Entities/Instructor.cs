namespace MentalHealthcare.Domain.Entities;

public class Instructor : ContentCreatorBe
{
    public int InstructorId { get; set; }


    public ICollection<Course>? Courses { get; set; }
}