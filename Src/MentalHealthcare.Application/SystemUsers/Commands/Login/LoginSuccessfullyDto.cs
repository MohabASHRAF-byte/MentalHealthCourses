namespace MentalHealthcare.Application.SystemUsers.Commands.Login;

public class LoginDto
{
    bool Success { get; set; }
    public string Name { get; set; } = default!;

    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } =default!;
}