using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetById;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Queries.GetAll
{
    public class GetAllPodcastSeriesQueryHandler(
         ILogger<GetAllPodcastSeriesQueryHandler> logger,
        IMapper mapper,
        IPodcastSeriesRepository SeriesRepo,
        IConfiguration configuration,
        IUserContext userContext,
        ILocalizationService localizationService)
        : IRequestHandler<GetAllPodcastSeriesQuery, PageResult<PodcastSeriesDto>>
    {
        public async Task<PageResult<PodcastSeriesDto>> Handle(GetAllPodcastSeriesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetAllInstructorsQuery");
            var currentUser = userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);
            int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;

            logger.LogInformation("Handling GetAllPodcastSeriesQuery");

            // Fetch all podcast series with pagination & filtering
            var seriesList = await SeriesRepo.GetAllPodcastSeriesAsync(request.SearchText, request.PageNumber, request.PageSize);

            // Convert to DTOs
            var seriesDtoList = mapper.Map<IEnumerable<PodcastSeriesDto>>(seriesList.Item2);

            return new PageResult<PodcastSeriesDto>(seriesDtoList, seriesList.Item1, request.PageNumber, request.PageSize);
        }
    }
}
