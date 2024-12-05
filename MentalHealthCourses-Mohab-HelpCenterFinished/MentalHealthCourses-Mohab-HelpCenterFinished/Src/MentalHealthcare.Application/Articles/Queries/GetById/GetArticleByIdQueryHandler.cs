using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Queries.GetById
{
    public class GetArticleByIdQueryHandler(
    ILogger<GetArticleByIdQueryHandler> logger,
    IMapper mapper,
    IArticleRepository articleRepository
) : IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {var Article = await articleRepository.GetArticleByIdAsync(request.Id);
            var ArticleDto = mapper.Map<ArticleDto>(Article); return ArticleDto;
        }
    }
}
