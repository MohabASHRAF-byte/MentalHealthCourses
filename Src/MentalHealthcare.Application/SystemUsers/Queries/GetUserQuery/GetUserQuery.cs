using MediatR;
using MentalHealthcare.Domain.Dtos.User;

namespace MentalHealthcare.Application.SystemUsers.Queries.GetUserQuery;

public class GetUserQuery : IRequest<GetUserProfile>
{
    public int UserId { get; set; }
}