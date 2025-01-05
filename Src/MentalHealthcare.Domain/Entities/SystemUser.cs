namespace MentalHealthcare.Domain.Entities;

public class SystemUser : HumanBe
{
    public int SystemUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public DateOnly BirthDate { get; set; }

    public List<Logs>? Logs { get; set; } = new();
    public List<Payments>? Payments { get; set; } = new();
    public ICollection<EnrollmentDetails>? EnrollmentDetails { get; set; }

    //
    public User User { get; set; }
}