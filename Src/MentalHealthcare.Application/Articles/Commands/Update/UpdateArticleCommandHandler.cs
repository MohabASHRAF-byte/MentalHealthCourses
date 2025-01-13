using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
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

namespace MentalHealthcare.Application.Articles.Commands.Update
{
    public class UpdateArticleCommandHandler(
    ILogger<UpdateArticleCommandHandler> logger,
    IArticleRepository arRepo,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<UpdateArticleCommand, int>
    {
        public async Task<int> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting update process for Article ID {AdId}.", request.ArticleId);

            // Authorize user
            logger.LogInformation("Authorizing user for updating Article.");
            var currentUser = userContext.EnsureAuthorizedUser(new List<UserRoles> { UserRoles.Admin }, logger);
            logger.LogInformation("User {UserId} authorized to update Articles.", currentUser.Id);

            // Validate Advertisement ID
            if (request.ArticleId == null)
            {
                logger.LogWarning("Article ID is null. Aborting update process.");
                return 0;
            }

            request.Images ??= new();
            ValidateImageSizes(request);

            // Retrieve Article
            logger.LogInformation("Fetching Article with ID: {AdId}.", request.ArticleId);
            var article = await arRepo.GetArticleByIdAsync((int)request.ArticleId);

            if (article == null)
            {
                logger.LogError("Article with ID {AdId} not found.", request.ArticleId);
                throw new KeyNotFoundException($"Article with ID {request.ArticleId} not found.");
            }

            // Update Article details
            logger.LogInformation("Updating details for Article ID: {AdId}.", request.ArticleId);
            UpdateArticleDetails(ref article, request);

            var bunnyClient = new BunnyClient(configuration);

            // Handle existing images
            logger.LogInformation("Handling existing images for Article ID: {AdId}.", request.ArticleId);
            HandleExistingImages(ref article, request, bunnyClient);

            // Upload new images
            logger.LogInformation("Uploading new images for Article ID: {AdId}.", request.ArticleId);
            UploadNewImages(ref article, request, bunnyClient);

            //// Check for at least one image
            //if (!article.ArticleImageUrls.Any())
            //{
            //    logger.LogWarning("No images found for Article ID: {AdId}. Marking as inactive.", request.ArticleId);
            //    Article.IsActive = false;
            //}

            // Persist changes
            logger.LogInformation("Saving changes for Article ID: {AdId}.", request.ArticleId);
            await arRepo.UpdateArticleAsync(article);

            logger.LogInformation("Article ID: {AdId} updated successfully.", request.ArticleId);
            return article.ArticleId;}


        private void ValidateImageSizes(UpdateArticleCommand request)
        {
            foreach (var image in request.Images)
            {
                var imageSizeInMb = image.Length / (1 << 20);
                if (imageSizeInMb > Global.ArticleImgSize)
                {
                    logger.LogWarning("Attempted to upload an image exceeding the allowed size: {ImageSize} MB.", imageSizeInMb);
                    throw new Exception($"Image size cannot exceed {Global.ArticleImgSize} MB.");
                }
            }
        }

        private void UpdateArticleDetails(ref Domain.Entities.Article article, UpdateArticleCommand request)
        {
        if (!request.Title.IsNullOrEmpty())
            {
                article.Title = request.Title!;
                logger.LogInformation("Updated Title for Article ID: {AdId} to {AdName}.", article.ArticleId, request.Title);
            }

            if (!request.Content.IsNullOrEmpty())
            {
                article.Content = request.Content!;
                logger.LogInformation("Updated Content for Advertisement ID: {AdId}.", article.ArticleId);
            }
        }

        private void HandleExistingImages(
      ref Domain.Entities.Article article,
      UpdateArticleCommand request,
      BunnyClient bunnyClient)
        {
            if (request.ImagesUrls != null && request.ImagesUrls.Count > 0)
            {
                var retainedImages = new List<ArticleImageUrl>();
                foreach (var image in article.ArticleImageUrls.ToList())
                {
                    var imageName = GetImageName(image.ImageUrl);
                    if (request.ImagesUrls.Contains(image.ImageUrl))
                    {
                        retainedImages.Add(image);
                        continue;
                    }

                    logger.LogInformation("Deleting image: {ImgName} for Article ID: {AdId}.", imageName, article.ArticleId);
                    bunnyClient.DeleteFileAsync(imageName, Global.ArticleFolderName).Wait();
                }

                arRepo.DeleteArticlePhotosUrlsAsync(article.ArticleId).Wait();
                article.ArticleImageUrls = retainedImages;
            }
            else
            {
                logger.LogWarning("No image URLs provided for Article ID: {AdId}. Removing all existing images.", article.ArticleId);
                article.ArticleImageUrls = new List<ArticleImageUrl>();
            }
        }



        private void UploadNewImages(
      ref Domain.Entities.Article article,
      UpdateArticleCommand request,
      BunnyClient bunnyClient)
        {
            foreach (var image in request.Images!)
            {
                var newImageName = $"{article.ArticleId}_{article.LastUploadImgCnt}.jpeg";
                article.LastUploadImgCnt++;
                var response = bunnyClient.UploadFileAsync(image, newImageName, Global.ArticleFolderName).Result;

                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning("Failed to upload image for Article ID: {AdId}. Error: {Message}", article.ArticleId, response.Message ?? "Unknown error");
                    continue;
                }

                logger.LogInformation("Successfully uploaded image for Article ID: {AdId}. URL: {Url}", article.ArticleId, response.Url);
                article.ArticleImageUrls.Add(new ArticleImageUrl
                {
                    ImageUrl = response.Url,
                    Article = article
                });
            }
        }


        private string GetImageName(string url) => url.Split('/').Last();


    }
}
