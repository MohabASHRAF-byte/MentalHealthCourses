using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
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

namespace MentalHealthcare.Application.Articles.Queries.GetById
{
    public class GetArticleByIdQueryHandler(
    ILogger<GetArticleByIdQueryHandler> logger,
    IArticleRepository arRepo,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Handling GetArticleByIdQuery for Article ID: {AdId}", request.ArticleID);

            // Authorize user
            logger.LogInformation("Authorizing user for retrieving Article by ID.");
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User {UserId} authorized to retrieve Article.", currentUser.Id);

            // Fetch Article
            logger.LogInformation("Fetching Article with ID: {AdId}", request.ArticleID);
            var ad = await arRepo.GetArticleByIdAsync(request.ArticleID);
            if (ad == null)
            {
                logger.LogWarning("Article with ID: {AdId} not found.", request.ArticleID);
                throw new KeyNotFoundException($"Advertisement with ID {request.ArticleID} not found.");
            }





            // Map to DTO
            logger.LogInformation("Mapping advertisement to DTO for Advertisement ID: {AdId}.", request.ArticleID);
            var ArticleDto = mapper.Map<ArticleDto>(ad);

            logger.LogInformation("Successfully retrieved and mapped Article with ID: {AdId}.", request.ArticleID);
            return ArticleDto;





        }
    }
}
