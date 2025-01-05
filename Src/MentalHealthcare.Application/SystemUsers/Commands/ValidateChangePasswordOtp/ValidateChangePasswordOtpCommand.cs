using System.Text.Json.Serialization;
using MediatR;

namespace MentalHealthcare.Application.SystemUsers.Commands.ValidateChangePasswordOtp;

public class ValidateChangePasswordOtpCommand : IRequest
{
    [JsonIgnore] public string? Tenant { get; set; }
    public string Email { get; set; } = default!;
    public string ResetCode { get; set; } = default!;
}