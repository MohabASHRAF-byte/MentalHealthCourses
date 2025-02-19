namespace MentalHealthcare.Domain.Dtos.User;

public class GetUserProfile
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateOnly BirthDate { get; set; }
}