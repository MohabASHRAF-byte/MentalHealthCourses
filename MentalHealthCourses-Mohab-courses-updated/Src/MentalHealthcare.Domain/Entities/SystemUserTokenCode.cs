namespace MentalHealthcare.Domain.Entities;

public class SystemUserTokenCode
{
    public Guid Id { get; set; }
    public User User { get; set; }
}