using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Utitlites.Jwt;
using MentalHealthcare.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.Refresh;

public class RefreshCommandHandler(
    ILogger<RefreshCommandHandler> logger,
    IJwtToken jwtToken
) : IRequestHandler<RefreshCommand, OperationResult<RefreshResponse>>
{
    public async Task<OperationResult<RefreshResponse>> Handle(RefreshCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Refresh @{token}", request.RefreshToken);
        var tokens = await jwtToken.RefreshTokens(request.RefreshToken);
        if (tokens == (null, null))
        {
            logger.LogInformation("Refresh @{token} failed", request.RefreshToken);
            return OperationResult<RefreshResponse>.Failure("Invalid refresh token", StateCode.Unauthorized);
        }

        var refreshedTokens = new RefreshResponse()
        {
            AccessToken = tokens.Item1,
            RefreshToken = tokens.Item2
        };
        logger.LogWarning("Refresh @{token} Success", request.RefreshToken);
        return OperationResult<RefreshResponse>.SuccessResult(refreshedTokens);
    }
}