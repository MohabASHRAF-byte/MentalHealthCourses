using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.Login;

public class LoginCommand : IRequest<OperationResult<LoginDto>>
{
    public string? Tenant { get; set; }
    public string UserIdentifier { get; set; }=default!;
    public string Password { get; set; }=default!;
    public string? Otp { get; set; }
}