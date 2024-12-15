using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Queries.GetAll
{
    public class GetAllArticlesQueryHandler(ILogger<GetAllArticlesQueryHandler> logger,
    IMapper mapper,
    IArticleRepository articleRepository) : IRequestHandler<GetAllArticlesQuery, PageResult<ArticleDto>>
    {
        public async Task<PageResult<ArticleDto>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
        {
            // TODO: add auth
            logger.LogInformation("Retrieving all courses with search text: {SearchText}, page number: {PageNumber}, page size: {PageSize}", request.SearchText, request.PageNumber, request.PageSize);


            // Retrieve all Articles from the repository
            var Articles = await articleRepository.GetAllArticlesAsync(request.SearchText, request.PageNumber, request.PageSize, request.sortBy);

            // Log the number of Articles retrieved
            logger.LogInformation("Retrieved {Count} courses.", Articles.Item1);

            // Map the retrieved Articles to DTOs
            var articleDtos = mapper.Map<IEnumerable<ArticleDto>>(Articles.Item2);

            // Create the page result
            var count = Articles.Item1;
            var ret = new PageResult<ArticleDto>(articleDtos, count, request.PageSize, request.PageNumber, request.sortBy);

            return ret;

        }
    }
}
