using MediatR;

namespace MentalHealthcare.Application.AdminUsers.Commands.Update;

public class UpdatePendingAdminCommand : IRequest
{
    public string OldEmail { get; set; } = default!;
    public string NewEmail { get; set; } = default!;
}