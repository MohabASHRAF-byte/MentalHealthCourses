using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Delete
{
    public class DeleteArticleCommandHandler(
 IConfiguration configuration,
 IUserContext userContext ,
 IArticleRepository articleRepository,
        ILogger<DeleteArticleCommandHandler> logger
        


        ) : IRequestHandler<DeleteArticleCommand>
    {
        public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Delete Article");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to delete Article by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to delete Article.");

            } 
            // TODO:Retrieve the article by ID
            var Article = await articleRepository.GetArticleByIdAsync(request.ArticleId);
            var bunny = new BunnyClient(configuration);
            foreach (var img in Article.PhotoUrl)
            {
                var imgName = GetImageName(Article.PhotoUrl);
                await bunny.DeleteFileAsync(imgName, Global.ArticleFolderName);
            }


            if (Article is null)
            {
                logger.LogWarning("Article with ID {ArticleId} not found.", request.ArticleId);
                throw new ResourceNotFound(nameof(Article), request.ArticleId.ToString());
            }



            await articleRepository.DeleteArticleAsync(request.ArticleId);



        }
        private string GetImageName(string url)
        {
            return url.Split('/').Last();
        }
    }
}
