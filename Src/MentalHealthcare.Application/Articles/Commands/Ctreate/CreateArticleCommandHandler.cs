using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Ctreate
{
    public class CreateArticleCommandHandler(
ILogger<CreateArticleCommandHandler> logger,
    IArticleRepository arRepo,
    IConfiguration configuration,
    IMapper mapper,
    IUserContext userContext
     ) : IRequestHandler<CreateArticleCommand, int>
    {
        public async Task<int> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {

            #region Authorize user
            logger.LogInformation("Authorizing user for creating advertisement.");
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User {UserId} authorized to create advertisements.", currentUser.Id);
            #endregion

            //TODO: Validate image sizes
            ValidateImageSizes(request);

            // Map to entity
            var newAr = mapper.Map<Domain.Entities.Article>(request);
            await arRepo.CreateArticleAsync(newAr);



            var bunnyClient = new BunnyClient(configuration);
            logger.LogInformation("Starting image uploads for Article ID: {ArId}", newAr.ArticleId);


            foreach (var img in request.Images)
            {
                var newImageName = $"{newAr.ArticleId}_{newAr.LastUploadImgCnt}.jpeg";
                newAr.LastUploadImgCnt++; // Increment image count for uniqueness

                // Upload image
                var response = await bunnyClient.UploadFileAsync(img, newImageName, Global.ArticleFolderName);

                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning("Failed to upload image for Article: {AdName}. Error: {Error}",
                        request.Title, response.Message ?? "Unknown error");
                    continue;
                }

                logger.LogInformation("Successfully uploaded image for Article ID: {AdId}. URL: {Url}",
                    newAr.ArticleId, response.Url);

                newAr.ArticleImageUrls.Add(new ArticleImageUrl
                {
                    ImageUrl = response.Url,
                    Article = newAr
                });
            }


            // Update advertisement status
            if (!newAr.ArticleImageUrls.Any())
            {
                logger.LogWarning("No images were successfully uploaded for Article ID: {AdId}. Marking as inactive.",
                    newAr.ArticleId);
            }

            await arRepo.UpdateArticleAsync(newAr);

            logger.LogInformation("Article ID: {AdId} created successfully with {ImageCount} images.",
                newAr.ArticleId, newAr.ArticleImageUrls.Count);
            return newAr.ArticleId;







        }


        private void ValidateImageSizes(CreateArticleCommand request)
        {
            foreach (var img in request.Images)
            {
                var imgSizeInMb = img.Length / (1 << 20); // Convert bytes to MB
                if (imgSizeInMb > Global.ArticleImgSize)
                {
                    logger.LogWarning("Image size validation failed. Image size: {SizeInMb} MB, Allowed size: {MaxSize} MB",
                        imgSizeInMb, Global.ArticleImgSize);
                    throw new ArgumentException($"Image size cannot be greater than {Global.ArticleImgSize} MB");
                }
            }
        }







    }
}