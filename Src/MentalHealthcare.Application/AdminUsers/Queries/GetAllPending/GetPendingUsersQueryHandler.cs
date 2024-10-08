using AutoMapper;
using MediatR;
using MentalHealthcare.Application.AdminUsers.Commands.Add;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;

public class GetPendingUsersQueryHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IMapper mapper
) : IRequestHandler<GetPendingUsersQuery, PageResult<PendingUsersDto>>
{
    public async Task<PageResult<PendingUsersDto>> Handle(GetPendingUsersQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all PendingUsers.");
        var pendingUsers =
            await adminRepository.GetAllAsync(request.SearchText, request.PageNumber, request.PageSize);
        var usersDtos = mapper.Map<IEnumerable<PendingUsersDto>>(pendingUsers.Item2);
        var count = pendingUsers.Item1;
        var ret = new PageResult<PendingUsersDto>(usersDtos, count, request.PageSize, request.PageNumber);
        return ret;
    }
}