using MediatR;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Queries.GetUserQuery;

public class GetUserQueryHandler(
    ILogger<GetUserQueryHandler> logger,
    IUserRepository userRepository,
    IUserContext userContext
) : IRequestHandler<GetUserQuery, GetUserProfile>
{
    public async Task<GetUserProfile> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // Log the start of the request handling
        logger.LogInformation("Handling GetUserQuery for UserId: {UserId}", request.UserId);
        try
        {
            // Ensure the user has the required authorization (only Admins can proceed)
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User is authorized to retrieve user details.");

            // Retrieve the user profile from the repository
            var user = await userRepository.GetByIdAsync(request.UserId);

            // Log the successful retrieval of the user profile

            logger.LogInformation("Successfully retrieved user with UserId: {UserId}", request.UserId);
            
            // Return the retrieved user profile
            return user;
        }
        catch (Exception ex)
        {
            // Log any errors encountered during the request
            logger.LogError(ex, "An error occurred while retrieving user with UserId: {UserId}", request.UserId);
            throw;
        }
    }
}