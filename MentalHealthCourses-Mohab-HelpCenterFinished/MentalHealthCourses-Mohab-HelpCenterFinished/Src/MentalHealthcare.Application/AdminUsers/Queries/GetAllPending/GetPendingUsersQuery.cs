using System.ComponentModel.DataAnnotations;
using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;

public class GetPendingUsersQuery:IRequest<PageResult<PendingUsersDto>>
{
    [MaxLength(100)]
    public string? SearchText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}