using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.Register;

public class RegisterCommand : IRequest<OperationResult<UserDto>>
{
    public string? Tenant { get; set; }=default!;
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateOnly Birthday { get; set; }
    public bool Active2Fa { get; set; }
}