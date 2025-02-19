using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos.User;

namespace MentalHealthcare.Application.SystemUsers.Queries.GerUsersQuery;

public class GetUsersQuery : IRequest<PageResult<GetUserProfile>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchText { get; set; } = "";
}