using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using System.Collections.Generic;

namespace MentalHealthcare.Application.Podcasts.Queries.GetAll
{
    public class GetAllPodcastsQuery : IRequest<PageResult<PodCastDto>>
    {
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
    }
}
