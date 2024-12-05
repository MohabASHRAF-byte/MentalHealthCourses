using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.SystemUsers.Commands.AddRoles;

public class AddRolesCommand:IRequest<OperationResult<string>>
{
    public string UserName { get; set; } = default!;
    public List<UserRoles> Roles { get; set; } = default!;
}