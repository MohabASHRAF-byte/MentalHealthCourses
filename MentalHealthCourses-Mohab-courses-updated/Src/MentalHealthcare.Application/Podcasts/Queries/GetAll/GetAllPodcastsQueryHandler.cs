using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcasts.Queries.GetAll
{
    public class GetAllPodcastsQueryHandler : IRequestHandler<GetAllPodcastsQuery, PageResult<PodCastDto>>
    {
        private readonly ILogger<GetAllPodcastsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IPodCastRepository _podcastRepository;

        public GetAllPodcastsQueryHandler(ILogger<GetAllPodcastsQueryHandler> logger, IMapper mapper, IPodCastRepository podcastRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _podcastRepository = podcastRepository;
        }

        public async Task<PageResult<PodCastDto>> Handle(GetAllPodcastsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all podcasts with search text: {SearchText}, page number: {PageNumber}, page size: {PageSize}",
                request.SearchText, request.PageNumber, request.PageSize);

            var podcasts = await _podcastRepository.GetAllPodcastsAsync(request.SearchText, request.PageNumber, request.PageSize, request.SortBy);

            _logger.LogInformation("Retrieved {Count} podcasts.", podcasts.Item1);

            var podcastDtos = _mapper.Map<IEnumerable<PodCastDto>>(podcasts.Item2);
            var result = new PageResult<PodCastDto>(podcastDtos, podcasts.Item1, request.PageSize, request.PageNumber, request.SortBy);

            return result;
        }
    }
}
