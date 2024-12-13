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

namespace MentalHealthcare.Application.Articles.Commands.AddArticle
{/// <summary> 
 /// Handler for adding a new article. 
 /// </summary>
 /// <param name="Ar_Repository">Repository for accessing articles.</param> 
 /// <param name="configuration">Application configuration settings.</param> 
 /// <param name="mapper">Mapper for converting between models and entities.</param> 
 /// <param name="logger">Logger for logging information and errors.</param> 
 /// <param name="mediator">Mediator for handling requests.</param>
    public class AddArticleCommandHandler(
    IArticleRepository Ar_Repository,
    IConfiguration configuration,
    // IConfiguration configuration,
    //UserContext userContext,
  //  IAdminRepository adminRepository,
    IMapper mapper,
   ILogger<AddArticleCommandHandler> logger,
    IMediator mediator) : IRequestHandler<AddArticleCommand, int>
    {
    


 async Task<int> IRequestHandler<AddArticleCommand, int>.Handle(AddArticleCommand request, CancellationToken cancellationToken)
    {
                 //TODO : Check Authentication
            //  var currentUser = userContext.GetCurrentUser();
            //if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            //  logger.LogError("Unauthorized access attempt by user.");
            //throw new UnauthorizedAccessException();
            //var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);
            try{
            logger.LogInformation("Creating article with title {Title}", request.Title);
            CheckPhotosSize(ref request);
            var NewArticle = mapper.Map<Article>(request);
            await Ar_Repository.CreateAsync(NewArticle);
            var bunny = new BunnyClient(configuration);
           foreach (var img in request.Image_Article)
    {var newImageName = $"{NewArticle.ArticleId}_{NewArticle.PhotoUrl}.jpeg";
// Upload the image to BunnyCDN
    var response = await bunny.UploadFile(img, newImageName, Global.ArticleFolderName);
if (!response.IsSuccessful || response.Url == null)
 {logger.LogWarning(@"Could not upload Article {ad} error msg :{mg}", request.Title,response.Message ??"");
   continue;}
                    if (string.IsNullOrEmpty(NewArticle.PhotoUrl)) 
                    { NewArticle.PhotoUrl = response.Url; }

                }
            await Ar_Repository.UpdateArticleAsync(NewArticle);
            return NewArticle.ArticleId;
                }
            catch (Exception ex) 
{ logger.LogError(ex, "An error occurred while creating the article"); throw; }

    }

             void CheckPhotosSize(ref AddArticleCommand request)
            {
                foreach (var img in request.Image_Article)
                {
                    var imgSizeInMb = img.Length / (1 << 20);
                    if (imgSizeInMb > Global.ArticleImgSize)
                    {
                        logger.LogWarning($"try to upload img with size {imgSizeInMb} ");
                        throw new Exception($"Image size cannot be greater than {Global.ArticleImgSize} MB");
                    }
                }
            }



    }
}
