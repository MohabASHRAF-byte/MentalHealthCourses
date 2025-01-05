namespace MentalHealthcare.Domain.Dtos.User;

public class UserProfileDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string userName { get; set; }
    public DateOnly BirthDate { get; set; }
    
    public long SecondsSinceUpdate { get; set; }
    public long SecondsSinceCreate { get; set; }
}