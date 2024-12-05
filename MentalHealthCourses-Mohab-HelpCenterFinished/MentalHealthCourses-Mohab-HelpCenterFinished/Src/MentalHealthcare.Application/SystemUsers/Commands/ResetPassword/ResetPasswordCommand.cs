using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResetPassword;

public class ResetPasswordCommand:IRequest<OperationResult<string>>
{
    public string? Tenant { get; set; }
    public string Email { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ResetCode { get; set; } = default!;
}