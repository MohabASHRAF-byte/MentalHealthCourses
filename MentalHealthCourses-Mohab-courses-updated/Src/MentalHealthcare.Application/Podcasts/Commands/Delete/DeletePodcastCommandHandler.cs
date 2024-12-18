using MediatR;
using MentalHealthcare.Application.Articles.Commands.DeleteArticle;
using MentalHealthcare.Application.BunnyServices;
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

namespace MentalHealthcare.Application.Podcasts.Commands.Delete
{
    public class DeletePodcastCommandHandler(
 IConfiguration configuration,
 IPodCastRepository PodRepo,
        ILogger<DeletePodcastCommandHandler> logger
        // UserContext userContext,
        //   IAdminRepository adminRepository,


        ) : IRequestHandler<DeletePodcastCommand>
    {
        public async Task Handle(DeletePodcastCommand request, CancellationToken cancellationToken)
        {
            //TODO : Check Authentication
            //  var currentUser = userContext.GetCurrentUser();
            //if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            //  logger.LogError("Unauthorized access attempt by user.");
            //throw new UnauthorizedAccessException();
            //var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

            logger.LogInformation("Delete Podcast");
            // TODO:Retrieve the article by ID
            var Podcast = await PodRepo.GetPodcastByIdAsync(request.podcastId);
            var bunny = new BunnyClient(configuration);
       
                var PodcastName = GetPodcastName(Podcast.Title);
                await bunny.DeleteFileAsync(PodcastName, Global.PodCasterFolderName);
       


            if (Podcast is null)
            {
                logger.LogWarning("Podcast with ID {PodcastId} not found.", request.podcastId);
                throw new ResourceNotFound(nameof(Podcast), request.podcastId.ToString());
            }



            await PodRepo.DeletePodcastAsync(request.podcastId);
        }





        private string GetPodcastName(string url)
        {
            return url.Split('/').Last();
        }



    }
}
