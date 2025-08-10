using AutoMapper;
using MediatR;
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

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetById
{
    public class GetPodcastSeriesByIdQueryHandler(
         ILogger<GetPodcastSeriesByIdQueryHandler> logger,
        IMapper mapper,
        IPodcastSeriesRepository SeriesRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService)
        : IRequestHandler<GetPodcastSeriesByIdQuery, PodcastSeriesDto>
    {
        public async Task<PodcastSeriesDto> Handle(GetPodcastSeriesByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Handling GetPodcastSeriesByIdQuery for PodcastSeries ID: {AdId}", request.PodcastSeriesId);

            // Authorize user
            logger.LogInformation("Authorizing user for retrieving PodcastSeries by ID.");
            var currentUser = userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);
            int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;
            // Fetch advertisement
            logger.LogInformation("Fetching advertisement with ID: {AdId}", request.PodcastSeriesId);
            var ad = await SeriesRepo.GetPodcastSeriesByIdAsync(request.PodcastSeriesId);
            if (ad == null)
            {
                logger.LogWarning("PodcastSeries with ID: {AdId} not found.", request.PodcastSeriesId);
                throw new ResourceNotFound(
                    "Advertisement",
                    "السسلسه",
                    request.PodcastSeriesId.ToString());
            }

            // Map to DTO
            logger.LogInformation("Mapping PodcastSeries to DTO for PodcastSeries ID: {AdId}.", request.PodcastSeriesId);
            var adDto = mapper.Map<PodcastSeriesDto>(ad);

            logger.LogInformation("Successfully retrieved and mapped PodcastSeries with ID: {AdId}.", request.PodcastSeriesId);
            return adDto;
        }
    }
}
