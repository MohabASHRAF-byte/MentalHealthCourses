using AutoMapper;
using MediatR;
using MentalHealthcare.Application.AdminUsers.Commands.Add;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.AdminUsers.Queries.GetAllPending;

/// <summary>
/// Handles the retrieval of all pending admin users.
/// </summary>
/// <param name="request">The query containing search parameters and pagination details.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>A task representing the page result of pending users.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the start of the retrieval process for pending users.</description>
/// </item>
/// <item>
/// <description>Fetch all pending users: 
/// <list type="bullet">
/// <item>Call <c>GetAllAsync</c> on the admin repository with search text, page number, and page size.</item>
/// <item>Log the number of pending users retrieved.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Map the retrieved pending users to DTOs: 
/// <list type="bullet">
/// <item>Use <c>mapper</c> to convert the pending users to <c>PendingUsersDto</c> objects.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>Create and return the page result: 
/// <list type="bullet">
/// <item>Construct a <c>PageResult</c> object with the DTOs, total count, page size, and page number.</item>
/// <item>Return the page result to the caller.</item>
/// </list>
/// </description>
/// </item>
/// </list>
/// </remarks>
public class GetPendingUsersQueryHandler(
    ILogger<AddAdminCommandHandler> logger,
    IAdminRepository adminRepository,
    IMapper mapper
) : IRequestHandler<GetPendingUsersQuery, PageResult<PendingUsersDto>>
{
    public async Task<PageResult<PendingUsersDto>> Handle(GetPendingUsersQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all pending users with search text: {SearchText}, page number: {PageNumber}, page size: {PageSize}", 
            request.SearchText, request.PageNumber, request.PageSize);
        
        var pendingUsers = await adminRepository.GetAllAsync(request.SearchText, request.PageNumber, request.PageSize);
        
        logger.LogInformation("Retrieved {Count} pending users.", pendingUsers.Item1);
        
        var usersDtos = mapper.Map<IEnumerable<PendingUsersDto>>(pendingUsers.Item2);
        var count = pendingUsers.Item1;
        
        var ret = new PageResult<PendingUsersDto>(usersDtos, count, request.PageSize, request.PageNumber);
        
        logger.LogInformation("Returning a page result with {UserCount} users on page {PageNumber}.", usersDtos.Count(), request.PageNumber);
        
        return ret;
    }
}
