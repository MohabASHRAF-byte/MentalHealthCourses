using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articles.Queries.GetById;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcasts.Queries.GetById
{
    public class GetByIdPodcastsQueryHandler(
    ILogger<GetByIdPodcastsQueryHandler> logger,
    IMapper mapper,
    IPodCastRepository podcastRepository
) : IRequestHandler<GetByIdPodcastsQuery, PodCastDto>
    {
        public async Task<PodCastDto> Handle(GetByIdPodcastsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving podcast with ID {PodcastId}", request.PodcastId);
            var podcast = await podcastRepository.GetPodcastByIdAsync(request.PodcastId);
            if (podcast == null) { logger.LogWarning("Podcast with ID {PodcastId} not found", request.PodcastId); throw new KeyNotFoundException($"Podcast with ID {request.PodcastId} not found."); }
            var podcastDto = mapper.Map<PodCastDto>(podcast); return podcastDto;
        }
    }
}
