using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<OperationResult<string>>
{
    public string? Tenant { get; set; }
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}