using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
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
    internal class GetAllArticlesQueryHandler(
    ILogger<GetAllArticlesQueryHandler> logger,
    IArticleRepository arRepo,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetAllArticlesQuery, PageResult<ArticleDto>>
    {
        public async Task<PageResult<ArticleDto>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation(
                            "Starting Handle method for GetAllArticlesQuery. SearchText: {SearchText}, PageNumber: {PageNumber}, PageSize: {PageSize}",
                            request.SearchText, request.PageNumber, request.PageSize);

            var currentUser = userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);
            int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;

            logger.LogDebug("Calling repository to get all articles for UserId: {UserId}", userId);



            var Articles = await arRepo.GetAllArticlesAsync(request.SearchText, request.PageNumber, request.PageSize);

            logger.LogInformation("Articles fetched successfully. Total records: {TotalRecords}", Articles.Item1);

            // Map to DTOs
            logger.LogInformation("Mapping Articles to DTOs.");

            var ArticlesDto = mapper.Map<IEnumerable<ArticleDto>>(Articles.Item2);

            //logger.LogInformation("Returning PageResult with {TotalPages} total pages and {TotalRecords} total records",
            //    (int)Math.Ceiling((double)Articles.Item1 / request.PageSize), Articles.Item1);



            return new PageResult<ArticleDto>(ArticlesDto, Articles.Item1, request.PageSize, request.PageNumber);




        }
    }
}