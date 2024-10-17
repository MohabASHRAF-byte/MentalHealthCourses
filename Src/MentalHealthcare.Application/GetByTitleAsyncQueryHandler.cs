using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articls.Queries.GetAll;
using MentalHealthcare.Application.Articls.Queries.GetById;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Articls.Queries.GetByTitle
{

    public class GetAsyncByIDQueryHandler
    (ILogger<GetAll_Articles_Query> logger,
IArticleRepository articleRepository,
IMapper mapper) : IRequestHandler<GetByTitleAsyncQuery, OperationResult<ArticlesDto>>
    {
        public async Task<OperationResult<ArticlesDto>> Handle(GetByTitleAsyncQuery request, CancellationToken cancellationToken)
        {

            var Article = await articleRepository.GetById(request.ArticleId);
            if (Article == null)
            {
                throw new DllNotFoundException("Article Not Found.");
                // Return a failure result indicating that the article was not found
                return OperationResult<ArticlesDto>.Failure("Article not found.", StateCode.NotFound);
            }

            var articleDto = mapper.Map<ArticlesDto>(Article);


            return OperationResult<ArticlesDto>.SuccessResult(articleDto, "Article retrieved successfully.");


        }
    }
}

