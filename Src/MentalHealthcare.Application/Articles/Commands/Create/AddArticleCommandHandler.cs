using AutoMapper;
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

namespace MentalHealthcare.Application.Articles.Commands.Create
{
    public class AddArticleCommandHandler(
    IArticleRepository Ar_Repository,
    IConfiguration configuration,
    IUserContext userContext,

    IMapper mapper,
   ILogger<AddArticleCommandHandler> logger,
    IMediator mediator) : IRequestHandler<AddArticleCommand, int>
    {
        public async Task<int> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation(@"Creating Article with name {ad}", request.Title);

            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to add advertisement by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to add Article .");
            }
 try
            {
                CheckPhotosSize(ref request);
                var NewArticle = mapper.Map<Article>(request);
                await Ar_Repository.CreateAsync(NewArticle);
                var bunny = new BunnyClient(configuration);

                var newImageName = $"{NewArticle.ArticleId}_{NewArticle.PhotoUrl}.jpeg";
                // Upload the image to BunnyCDN
                var response = await bunny.UploadFileAsync(request.Image_Article, newImageName, Global.ArticleFolderName);
                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning(@"Could not upload Article {ad} error msg :{mg}", request.Title, response.Message ?? "");
                }
                if (string.IsNullOrEmpty(NewArticle.PhotoUrl))
                { NewArticle.PhotoUrl = response.Url; }


                await Ar_Repository.UpdateArticleAsync(NewArticle);
                return NewArticle.ArticleId;
            }
            catch (Exception ex)
            { logger.LogError(ex, "An error occurred while creating the article"); throw; }

        }

        void CheckPhotosSize(ref AddArticleCommand request)
        {

            var imgSizeInMb = request.Image_Article.Length / (1 << 20);
            if (imgSizeInMb > Global.ArticleImgSize)
            {
                logger.LogWarning($"try to upload img with size {imgSizeInMb} ");
                throw new Exception($"Image size cannot be greater than {Global.ArticleImgSize} MB");
            }




        }
    }
}
