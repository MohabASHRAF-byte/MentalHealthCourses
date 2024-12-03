using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.AdminUsers.Commands.Register;

public class RegisterAdminCommand:IRequest<OperationResult<string>>
{
    public string? Tenant { get; set; }=default!;
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool Active2Fa { get; set; }
}