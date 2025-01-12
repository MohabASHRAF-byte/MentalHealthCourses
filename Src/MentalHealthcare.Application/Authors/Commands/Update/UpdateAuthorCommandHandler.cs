using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Update;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MentalHealthcare.Application.Authors.Commands.Update
{
    public class UpdateAuthorCommandHandler(
    ILogger<UpdateAuthorCommandHandler> logger,
    IAuthorRepository auRepo,
    IMapper mapper,
    IConfiguration configuration,
    IUserContext userContext
) : IRequestHandler<UpdateAuthorCommand, int>
    {
        public async Task<int> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation($"Starting update process for Author ID {request.AuthorId}.");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to update Author by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to update Author.");
            }



            if (request.AuthorId == null)
            {

                logger.LogWarning("Author ID cannot be null.");
                return 0;
            }

            if (request.ImagesUrl == null)
            {throw new Exception("Image cannot be null.");}

            ValidateImageSizes(request.ImagesUrl);

            var Auth = await auRepo.GetAuthorById((int)request.AuthorId);
            UpdateAuthorDetails(ref Auth, request);

            var bunnyClient = new BunnyClient(configuration);

            HandleExistingImages(ref Auth, request, bunnyClient);
            UploadNewImages(ref Auth, request, bunnyClient);

            await auRepo.UpdateAuthorAsync(Auth);
            return Auth.AuthorId;
        }
        private void UpdateAuthorDetails(ref Domain.Entities.Author autor, UpdateAuthorCommand request)
        {
            if (!request.AuthorName.IsNullOrEmpty())
                autor.Name = request.AuthorName!;

            if (!request.AuthorAbout.IsNullOrEmpty())
                autor.Name = request.AuthorAbout!;


        }
        private void ValidateImageSizes(IFormFile image)
        {
            var imageSizeInMb = image.Length / (1 << 20); // Convert bytes to MB
            if (imageSizeInMb > Global.AuthorImgSize)
            {
                logger.LogWarning($"Attempted to upload an image exceeding the allowed size: {imageSizeInMb} MB.");
                throw new Exception($"Image size cannot exceed {Global.AuthorImgSize} MB.");
            }
        }



        private void HandleExistingImages(
       ref Domain.Entities.Author author,
       UpdateAuthorCommand request,
       BunnyClient bunnyClient)
        {

            if (request.ImagesUrl == null)
            {
                logger.LogInformation("No new image provided. Retaining the existing image.");
                return;
            }



            //TODO : Check if the author already has an image
            if (author.ImageUrl is not null)
            {
                var currentImageName = GetImageName(author.ImageUrl);

                bunnyClient.DeleteFileAsync(currentImageName, Global.AuthorFolderName).Wait();
                author.ImageUrl = null;

                //   auRepo.DeleteAuthorImageAsync(author.AuthorId);
                //author.ImageUrl = null; // Reset the current image URL
            }


        }





        private void UploadNewImages(
      ref Domain.Entities.Author author,
      UpdateAuthorCommand request,
      BunnyClient bunnyClient)
        {
            
                var newImageName = $"{author.AuthorId}.jpeg";
                var response = bunnyClient.UploadFileAsync(request.ImagesUrl, newImageName, Global.AuthorFolderName).Result;

                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning(
                        "Failed to upload image for Author {Name}. Error: {Message}",
                        request.AuthorName,
                        response.Message ?? ""
                    );
                throw new Exception("Failed to upload the new image.");

            }
            author.ImageUrl = response.Url;

            
            



        }











        private string GetImageName(string url) => url.Split('/').Last();

    }
}
