using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommandHandler(
 IConfiguration configuration,
 IArticleRepository articleRepository,
        ILogger<DeleteArticleCommandHandler> logger
        // UserContext userContext,
        //   IAdminRepository adminRepository,


        ) : IRequestHandler<DeleteArticleCommand>

    {
        public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {        //TODO : Check Authentication
            //  var currentUser = userContext.GetCurrentUser();
            //if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            //  logger.LogError("Unauthorized access attempt by user.");
            //throw new UnauthorizedAccessException();
            //var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

            logger.LogInformation("Delete Article");
              // TODO:Retrieve the article by ID
            var Article = await articleRepository.GetArticleByIdAsync(request.ArticleId);
            var bunny = new BunnyClient(configuration);
            foreach (var img in Article.PhotoUrl)
            {
                var imgName = GetImageName(Article.PhotoUrl);
                await bunny.DeleteFile(imgName, Global.ArticleFolderName);
            }


 if (Article is null || !await articleRepository.IsExistByTitle(request.title))
            {logger.LogWarning("Article with ID {ArticleId} not found.", request.ArticleId);
             throw new ResourceNotFound(nameof(Article), request.ArticleId.ToString());}
           


            await articleRepository.DeleteArticleAsync(request.ArticleId);

           





            
        }
        private string GetImageName(string url)
        {
            return url.Split('/').Last();
        }
    }
}
