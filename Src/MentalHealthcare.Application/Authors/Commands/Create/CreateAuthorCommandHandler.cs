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
using System.Threading;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Commands.Create
{
    public class CreateAuthorCommandHandler(
        ILogger<CreateAuthorCommandHandler> _logger,
        IAuthorRepository auRepo ,
        IUserContext _userContext,
        IMapper _mapper,
        IConfiguration _configuration,
        IAdminRepository adminRepository
        
        ) : IRequestHandler<CreateAuthorCommand, int>

    {


        public async Task<int> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            // Fetch the current admin user from the context
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                _logger.LogWarning("Unauthorized attempt to add Author by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have permission to add Author.");
            }


            CheckPhotosSize(ref request);


            var newAu = _mapper.Map<Author>(request);

            await auRepo.AddAuthorAsync(newAu); 
           


            // If an image is provided, upload it using the Bunny client
            var bunny = new BunnyClient(_configuration);
            if (request.ImageUrl != null)
            {
                var newImageName = $"{newAu.AuthorId}.jpeg";
                var response = await bunny.UploadFileAsync(request.ImageUrl, newImageName, Global.AuthorFolderName);
                if (response.IsSuccessful && response.Url != null)
                {
                    newAu.ImageUrl = response.Url;
                }
                else
                {
                    _logger.LogWarning("Could not upload Author {ad}, error msg: {mg}", request.Name, response.Message ?? "");
                }
            }


            return newAu.AuthorId;
        }

        private void CheckPhotosSize(ref CreateAuthorCommand request)
        {
            if (request.ImageUrl != null)
            {
                var imgSizeInMb = request.ImageUrl.Length / (1 << 20);
                if (imgSizeInMb > Global.AuthorImgSize)
                {
                    _logger.LogWarning($"Trying to upload image with size {imgSizeInMb}MB");
                    throw new Exception($"Image size cannot be greater than {Global.AuthorImgSize}MB");
                }
            }
        }
    }









}

