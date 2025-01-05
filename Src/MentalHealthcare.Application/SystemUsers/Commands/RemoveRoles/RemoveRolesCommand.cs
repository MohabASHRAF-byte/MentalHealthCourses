using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.SystemUsers.Commands.RemoveRoles;

public class RemoveRolesCommand:IRequest<OperationResult<string>>
{
    public string UserName { get; set; } = default!;
}