using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Articls.Queries.GetAll
{
    public class GetAll_Articles_QueryHandler(
    ILogger<GetAll_Articles_Query> logger,
    IArticleRepository articleRepository,
    IMapper mapper) : IRequestHandler<GetAll_Articles_Query, PageResult<ArticlesDto>>
    {
        public async Task<PageResult<ArticlesDto>> Handle(GetAll_Articles_Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving all Articles.");

            var AllArticles =
          await articleRepository.GetAllAsyncArticles(request.SearchText, request.PageNumber, request.PageSize);
            var ArticlesDto2 = mapper.Map<IEnumerable<ArticlesDto>>(AllArticles.Item2);

            var count = AllArticles.Item1;
            var ret = new PageResult<ArticlesDto>(ArticlesDto2, count, request.PageSize, request.PageNumber);
            return ret;



            //    adminRepository.GetAllAsync(request.SearchText, request.PageNumber, request.PageSize);
            //var usersDtos = mapper.Map<IEnumerable<PendingUsersDto>>(pendingUsers.Item2);
            //var count = pendingUsers.Item1;
            //var ret = new PageResult<PendingUsersDto>(usersDtos, count, request.PageSize, request.PageNumber);
            //return ret;




        }
    }
}
