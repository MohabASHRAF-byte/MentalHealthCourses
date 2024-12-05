using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.Articles.Commands.DeleteArticle;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandHandler(
 IConfiguration configuration,
 IArticleRepository AR_Repository,
        ILogger<DeleteArticleCommandHandler> logger
        // UserContext userContext,
        //   IAdminRepository adminRepository,


        ) : IRequestHandler<UpdateArticleCommand, int>
    {
        public async Task<int> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {


            logger.LogInformation($"Starting update process for Article ID {request.ArticleId}.");

            if (request.ArticleId == null)
                return 0;



            request.Image_Article ??= new();
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
            foreach (var image in request.Image_Article)
            {
                var imageSizeInMb = image.Length / (1 << 20);
                if (imageSizeInMb > Global.ArticleImgSize)
                {
                    logger.LogWarning($"Attempted to upload an image exceeding the allowed size: {imageSizeInMb} MB.");
                    throw new Exception($"Image size cannot exceed {Global.ArticleImgSize} MB.");
                }
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
            foreach (var image in request.Image_Article!)
            {
                var newImageName = $"{article.ArticleId}.jpeg";
                var response = bunnyClient.UploadFile(image, newImageName, Global.ArticleFolderName).Result;

                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning(
                        "Failed to upload image for Article {ArticleName}. Error: {Message}",
                        request.Title,
                        response.Message ?? ""
                    );
                    continue;
                }
           // Assuming you want to set the first successfully uploaded image URL
                article.PhotoUrl = response.Url; 

            }

         }
                private string GetImageName(string url) => url.Split('/').Last();


    }
}


/*
   public async Task<int> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {

            var article = await articleRepository.GetArticleByIdAsync(request.ArticleId);
            logger.LogInformation($"Starting update process for Article ID {request.ArticleId}.");

            if (request.ArticleId == null)
            {  return 0;}  

           
            article.Content = request.Content;
            article.Title = request.Title; 
            article.CreatedDate = request.CreatedDate; 
            article.Author = request.Author;
            if (request.photo != null)
            {  using var memoryStream = new MemoryStream(); 
                await request.photo.CopyToAsync(memoryStream); 
                article.PhotoUrl = Convert.ToBase64String(memoryStream.ToArray()); // Example of handling the photo
            }

            await articleRepository.UpdateArticleAsync(article);
            logger.LogInformation("Article with ID {ArticleId} updated successfully.", request.ArticleId); return article.ArticleId;





        } 
 */