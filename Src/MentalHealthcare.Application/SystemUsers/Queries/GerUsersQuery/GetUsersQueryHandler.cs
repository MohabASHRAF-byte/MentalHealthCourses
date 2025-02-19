using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Queries.GerUsersQuery;

public class GetUsersQueryHandler(
    ILogger<GetUsersQueryHandler> logger,
    IUserRepository userRepository,
    IUserContext userContext
) : IRequestHandler<GetUsersQuery, PageResult<GetUserProfile>>
{
    public async Task<PageResult<GetUserProfile>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // Log the incoming request details
        logger.LogInformation("Handling GetUsersQuery | PageNumber: {PageNumber}, PageSize: {PageSize}, SearchText: {SearchText}", 
            request.PageNumber, request.PageSize, request.SearchText ?? "N/A");

        try
        {
            // Ensure the user has the required authorization
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User is authorized to fetch users.");

            // Retrieve users from the repository
            var (total, users) = await userRepository.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchText
            );

            logger.LogInformation("Successfully retrieved {TotalUsers} users.", users.Count);

            // Create a paginated result object
            var results = new PageResult<GetUserProfile>(users, total, request.PageSize, request.PageNumber);
            
            // Log pagination details
            logger.LogInformation("Returning paginated result: Page {CurrentPage} of {TotalPages}", 
                request.PageNumber, results.TotalPages);

            return results;
        }
        catch (Exception ex)
        {
            // Log any unexpected errors that occur
            logger.LogError(ex, "Error occurred while fetching users.");
            throw;
        }
    }
}
