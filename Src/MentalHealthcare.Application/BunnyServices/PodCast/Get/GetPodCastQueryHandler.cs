using MediatR;
using MentalHealthcare.Application.BunnyServices.PodCast.Token;
using MentalHealthcare.Domain.Dtos;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.BunnyServices.PodCast.Get;

public class GetPodCastQueryHandler(
    IConfiguration configuration
    ) : IRequestHandler<GetPodCastQuery, PodCastDto>
{
    public async Task<PodCastDto> Handle(GetPodCastQuery request, CancellationToken cancellationToken)
    {
        var pullZone = configuration["BunnyCdn:PullZone"]!;
        var storageKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
        var expiryTime = DateTimeOffset.UtcNow.AddHours(4);
        var signedUrl = TokenSigner.SignUrl(t =>
        {
            t.Url = $"https://{pullZone}.b-cdn.net/{request.PodCastId}";
            t.SecurityKey = storageKey;
            t.ExpiresAt = expiryTime;
            t.TokenPath = "/";
        });
        return new PodCastDto()
        {
            Url = signedUrl
        };
    }
}