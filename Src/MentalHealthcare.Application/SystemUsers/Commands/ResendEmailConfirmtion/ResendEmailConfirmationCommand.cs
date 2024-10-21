using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.ResendEmailConfirmtion;

public class ResendEmailConfirmationCommand : IRequest<OperationResult<string>>
{
    public string? Tenant { get; set; }
    public string Email { get; set; } = default!;
}