using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.Episodes.Queries.GetAll
{
    public class GetAllEpisodesQueryHandler(
        ILogger<GetAllEpisodesQueryHandler> logger,
        IMapper mapper,
        IPodcastRepository podcastRepository,
        IUserContext userContext,
        ILocalizationService localizationService)
        : IRequestHandler<GetAllEpisodesQuery, PageResult<PodCastDto>>
    {
        public async Task<PageResult<PodCastDto>> Handle(GetAllEpisodesQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Handling GetAllInstructorsQuery");
            var currentUser = userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);
            int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;
            logger.LogInformation("Fetching episodes with filters: SearchText = {SearchText}, PodcastSeriesId = {PodcastSeriesId}, PageNumber = {PageNumber}, PageSize = {PageSize}",
                request.SearchText, request.PodcastSeriesId, request.PageNumber, request.PageSize);

            // 🔹 Pass PodcastSeriesId to filter the episodes
            var episodes = await podcastRepository.GetAllEpisodesAsync(request.SearchText ,  request.PageNumber, request.PageSize ,request.PodcastSeriesId );
            var episodeDtos = mapper.Map<IEnumerable<PodCastDto>>(episodes.Item2);

            return new PageResult<PodCastDto>(episodeDtos, episodes.Item1, request.PageNumber, request.PageSize);
        }
    }
}