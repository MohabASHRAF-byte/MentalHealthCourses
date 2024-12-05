using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.ChangePassword;

public class ChangePasswordCommand:IRequest<OperationResult<string>>
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}