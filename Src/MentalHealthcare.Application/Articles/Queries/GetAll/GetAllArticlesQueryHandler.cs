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

            logger.LogInformation("Handling GetAllArticlesQuery with SearchText: {SearchText}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.SearchText, request.PageNumber, request.PageSize);


            // Authorize user
            logger.LogInformation("Authorizing user for retrieving all Articles.");
            var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
            logger.LogInformation("User {UserId} authorized to retrieve all Articles.", currentUser.Id);

            logger.LogInformation("Fetching Articles from the repository.");




            var Articles = await arRepo.GetAllArticlesAsync(request.SearchText , request.PageNumber, request.PageSize);

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