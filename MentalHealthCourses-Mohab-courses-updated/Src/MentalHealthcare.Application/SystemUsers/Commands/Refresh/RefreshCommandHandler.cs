using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Utitlites.Jwt;
using MentalHealthcare.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.Refresh;

/// <summary>
/// Handles the process of refreshing access tokens for authenticated users.
/// </summary>
/// <param name="request">The command containing the refresh token.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>An <see cref="OperationResult{RefreshResponse}"/> indicating the result of the refresh attempt.</returns>
/// 
/// <remarks>
/// The following describes the logic flow of the method:
/// <list type="number">
/// <item>
/// <description>Log the incoming refresh token.</description>
/// </item>
/// <item>
/// <description>Attempt to refresh the tokens using the provided refresh token.
/// <list type="bullet">
/// <item>If the tokens are invalid (null values), log the failure and return a failure result.</item>
/// </list>
/// </description>
/// </item>
/// <item>
/// <description>If successful, create a new <see cref="RefreshResponse"/> object with the new access and refresh tokens.</description>
/// </item>
/// <item>
/// <description>Log the successful refresh operation and return a success result with the new tokens.</description>
/// </item>
/// </list>
/// </remarks>
/// <exception cref="ApplicationException">
/// Thrown if the refresh token is invalid.
/// </exception>
public class RefreshCommandHandler(
    ILogger<RefreshCommandHandler> logger,
    IJwtToken jwtToken
) : IRequestHandler<RefreshCommand, OperationResult<RefreshResponse>>
{
    public async Task<OperationResult<RefreshResponse>> Handle(RefreshCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Refresh token: {Token}", request.RefreshToken);

        // Attempt to refresh tokens
        var tokens = await jwtToken.RefreshTokens(request.RefreshToken);
        
        // Check if the tokens are valid
        if (tokens == (null, null))
        {
            logger.LogWarning("Failed to refresh token: {Token}", request.RefreshToken);
            return OperationResult<RefreshResponse>.Failure("Invalid refresh token", StateCode.Unauthorized);
        }

        // Create a response object with the new tokens
        var refreshedTokens = new RefreshResponse()
        {
            AccessToken = tokens.Item1,
            RefreshToken = tokens.Item2
        };

        logger.LogInformation("Successfully refreshed token: {Token}", request.RefreshToken);
        return OperationResult<RefreshResponse>.SuccessResult(refreshedTokens);
    }
}
