using AutoMapper;
using Flurl.Util;
using MediatR;
using MentalHealthcare.Application.BunnyServices.PodCast.Token;
using MentalHealthcare.Application.Podcast.Episodes.Commands.AddNewEpisode;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Queries.GetById
{
    public class GetPodcastByIdQueryHandler(
       ILogger<GetPodcastByIdQueryHandler> logger,
        IMapper mapper,
        IPodcastRepository PodRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService



        ) : IRequestHandler<GetPodcastByIdQuery, PodCastDto>
    {
        public async Task<PodCastDto> Handle(GetPodcastByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPodcastByIdQuery for Podcast ID: {AdId}", request.PodCastid);

            // Authorize user
            var currentUser = userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);
            int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;

            // Fetch advertisement
            logger.LogInformation("Fetching Podcast with ID: {AdId}", request.PodCastid);
            var ad = await PodRepo.GetEpisodeByIdAsync(request.PodCastid);
            if (ad == null)
            {
                logger.LogWarning("PodcastSeries with ID: {AdId} not found.", request.PodCastid);
                throw new ResourceNotFound(
                    "PodCast",
                    "الحلقه",
                    request.PodCastid.ToString());
            }

            // Map to DTO
            logger.LogInformation("Mapping PodcastSeries to DTO for Podcast ID: {AdId}.", request.PodCastid);
            var adDto = mapper.Map<PodCastDto>(ad);

            logger.LogInformation("Successfully retrieved and mapped PodcastSeries with ID: {AdId}.", request.PodCastid);
            var pullZone = configuration["BunnyCdn:PullZone"]!;
            var storageKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
            var expiryTime = DateTimeOffset.UtcNow.AddHours(4);
            var signedUrl = TokenSigner.SignUrl(t =>
            {
                t.Url = $"https://{pullZone}.b-cdn.net/{request.PodCastid}";
                t.SecurityKey = storageKey;
                t.ExpiresAt = expiryTime;
                t.TokenPath = "/";
            });
            adDto.AudioFileUrl = signedUrl;
            return adDto;




        }
    }
}
