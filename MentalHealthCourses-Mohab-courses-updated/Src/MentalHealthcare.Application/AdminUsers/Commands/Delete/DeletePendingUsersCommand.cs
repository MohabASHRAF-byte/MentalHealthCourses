using MediatR;

namespace MentalHealthcare.Application.AdminUsers.Commands.Delete;

public class DeletePendingUsersCommand:IRequest
{
    
    public List<string> PendingUsers { get; set; } = new();
}