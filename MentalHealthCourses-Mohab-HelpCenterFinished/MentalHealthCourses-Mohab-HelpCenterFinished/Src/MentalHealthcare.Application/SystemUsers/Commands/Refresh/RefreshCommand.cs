using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.SystemUsers.Commands.Refresh;

public class RefreshCommand:IRequest<OperationResult<RefreshResponse>>
{
    public string RefreshToken { get; set; }=default!;
}