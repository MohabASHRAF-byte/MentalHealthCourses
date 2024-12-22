using MediatR;
using MentalHealthcare.Application.Articles.Commands.Delete;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Update
{
    public class UpdateArticleCommandHandler(
 IConfiguration configuration,
 IArticleRepository AR_Repository,
        ILogger<DeleteArticleCommandHandler> logger
   , IUserContext userContext
        //   IAdminRepository adminRepository,


        ) : IRequestHandler<UpdateArticleCommand, int>
    {
        public async Task<int> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation($"Starting update process for Article ID {request.ArticleId}.");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to update Article by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to update Article.");
            }
            if (request.ArticleId == null )
                return 0;
            if (request.Image_Article == null)
            {
                // Handle the null case here
                throw new ArgumentNullException(nameof(request.Image_Article), "Image_Article cannot be null.");
            }

            ValidateImageSizes(request);

            var Updated_Article = await AR_Repository.GetArticleByIdAsync((int)request.ArticleId);
            UpdateArticleDetails(ref Updated_Article, request);

            var bunnyClient = new BunnyClient(configuration);

            UploadNewImages(ref Updated_Article, request, bunnyClient);

            await AR_Repository.UpdateArticleAsync(Updated_Article);
            return Updated_Article.ArticleId;
        }


        private void ValidateImageSizes(UpdateArticleCommand request)
        {
            var imgSizeInMb = request.Image_Article.Length / (1 << 20);
            if (imgSizeInMb > Global.ArticleImgSize)
            {
                logger.LogWarning($"try to upload img with size {imgSizeInMb} ");
                throw new Exception($"Image size cannot be greater than {Global.ArticleImgSize} MB");
            }

        }


        private void UpdateArticleDetails(ref Domain.Entities.Article article, UpdateArticleCommand request)
        {

            if (!request.Title.IsNullOrEmpty())
                article.Title = request.Title!;

            if (!request.Content.IsNullOrEmpty())
                article.Content = request.Content!;
        }

        private void UploadNewImages(ref Domain.Entities.Article article,
      UpdateArticleCommand request,
      BunnyClient bunnyClient)
        {

            var newImageName = $"{article.ArticleId}.jpeg";
            var response = bunnyClient.UploadFileAsync(request.Image_Article, newImageName, Global.ArticleFolderName).Result;

            if (!response.IsSuccessful || response.Url == null)
            {
                logger.LogWarning(
                    "Failed to upload image for Article {ArticleName}. Error: {Message}",
                    request.Title,
                    response.Message ?? ""
                );
            }
            // Assuming you want to set the first successfully uploaded image URL
            article.PhotoUrl = response.Url;



        }


    }
}
