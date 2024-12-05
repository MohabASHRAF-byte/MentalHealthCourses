using MediatR;

namespace MentalHealthcare.Application.AdminUsers.Commands.Add;

public class AddAdminCommand:IRequest
{
    public string Email { get; set; }=default!;
    
}