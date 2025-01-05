using MediatR;

namespace MentalHealthcare.Application.SystemUsers.Profile.UpdateProfile;

public class UpdateProfileDataCommand : IRequest
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public DateOnly? BirthDate { get; set; } = null!;
}