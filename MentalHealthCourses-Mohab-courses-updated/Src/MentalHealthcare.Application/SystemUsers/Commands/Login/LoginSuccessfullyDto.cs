namespace MentalHealthcare.Application.SystemUsers.Commands.Login;

public class LoginDto
{
    bool Success { get; set; }
    public string Name { get; set; } = default!;
    public bool RequireOtp { get; set; } = false;
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}