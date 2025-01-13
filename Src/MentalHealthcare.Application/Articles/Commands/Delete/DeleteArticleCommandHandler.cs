using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Delete;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
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
    internal class DeleteArticleCommandHandler(
    ILogger<DeleteArticleCommandHandler> logger,
    IArticleRepository arRepo,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<DeleteArticleCommand>
    {
        public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {


            logger.LogInformation("Handling DeleteArticleCommand for Article ID: {AdId}",
            request.Id);

            // Authorize user
            logger.LogInformation("Authorizing user for deleting Article.");
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User {UserId} authorized to delete Article.", currentUser.Id);

            // Fetch advertisement
            logger.LogInformation("Fetching Article with ID: {AdId}", request.Id);
            var Ar = await arRepo.GetArticleByIdAsync(request.Id);
            if (Ar == null)
            {
                logger.LogWarning("Article with ID: {AdId} not found.", request.Id);
                throw new KeyNotFoundException($"Article with ID {request.Id} not found.");
            }


            var bunnyClient = new BunnyClient(configuration);



            // Delete images from BunnyCDN
            logger.LogInformation("Deleting images for Article ID: {AdId}", request.Id);
            foreach (var img in Ar.ArticleImageUrls)
            {
                var imgName = GetImageName(img.ImageUrl);
                var response = await bunnyClient.DeleteFileAsync(imgName, Global.ArticleFolderName);

                if (!response.IsSuccessful)
                {
                    logger.LogWarning("Failed to delete image: {ImgName} for Article ID: {AdId}. Error: {Error}",
                        imgName, request.Id, response.Message ?? "Unknown error");
                }
                else
                {
                    logger.LogInformation("Successfully deleted image: {ImgName} for Article ID: {AdId}", imgName,
                        request.Id);
                }
            }

            // Delete advertisement from the repository
            logger.LogInformation("Deleting Article record for Advertisement ID: {AdId}", request.Id);
            await arRepo.DeleteArticleAsync(request.Id);

            logger.LogInformation("Article ID: {AdId} deleted successfully.", request.Id);





        }


        private string GetImageName(string url)
        {
            return url.Split('/').Last();
        }








    }
}
