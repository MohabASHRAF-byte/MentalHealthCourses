using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.BunnyServices;
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

namespace MentalHealthcare.Application.Authors.Commands.Create
{
    public class CreateAuthorCommandHandler(
    ILogger<CreateAuthorCommandHandler> logger,
    IAuthorRepository authorRepository,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<CreateAuthorCommand, int>
    {
        public async Task<int> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {   //todo 
            // add auth

            CheckPhotosSize(ref request);
            var NewAuthor = mapper.Map<Domain.Entities.Author>(request);

            await authorRepository.AddAuthor(NewAuthor);

            var bunny = new BunnyClient(configuration);

           
                var newImageName = $"{NewAuthor.AuthorId}_{NewAuthor.ImageUrl}.jpeg";
                
                // Upload the image to BunnyCDN
                var response = await bunny.UploadFile(request.ImageUrl, newImageName, Global.AuthorFolderName);

                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning(@"Could not upload Author {ad} error msg :{mg}", request.Name,
                        response.Message ?? ""
                    );
                }

               
            


 await authorRepository.UpdateAuthorAsync(NewAuthor);

                return NewAuthor.AuthorId;

        }
        private void CheckPhotosSize(ref CreateAuthorCommand request)
        {if (request.ImageUrl != null)
            {var imgSizeInMb = request.ImageUrl.Length / (1 << 20);
             if (imgSizeInMb > Global.AuthorImgSize)
                {logger.LogWarning($"try to upload img with size {imgSizeInMb} ");
      throw new Exception($"Image size cannot be greater than {Global.AuthorImgSize} MB");}

           }
       else{throw new Exception("Image of Author is required.");}}}}
