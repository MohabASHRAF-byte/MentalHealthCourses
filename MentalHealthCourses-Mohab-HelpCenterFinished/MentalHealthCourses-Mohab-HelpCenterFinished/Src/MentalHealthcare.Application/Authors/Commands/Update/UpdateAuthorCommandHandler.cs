using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Authors.Commands.Update
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, int>
    {
        private readonly ILogger<UpdateAuthorCommandHandler> logger;
        private readonly IAuthorRepository authorRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public UpdateAuthorCommandHandler(
            ILogger<UpdateAuthorCommandHandler> logger,
            IAuthorRepository authorRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.authorRepository = authorRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<int> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting update process for Author ID {request.AuthorId}.");

            // Retrieve the existing author from the repository
            var author = await authorRepository.GetAuthorById(request.AuthorId);
            if (author == null)
            {
                logger.LogWarning($"Author with ID {request.AuthorId} not found.");
                throw new Exception("Author not found.");
            }

            // Update author details
            author.Name = request.Name;
            author.About = request.About;
           // author.AddedBy = request.AddedBy;

            // Handle the image URL
            if (request.ImageUrl != null)
            {
                // Optional: Handle image size validation here if needed

                // Assuming you have a method to upload the image to BunnyCDN
                var bunnyClient = new BunnyClient(configuration);
                var imageUploadResponse = await bunnyClient.UploadFile(request.ImageUrl, $"{author.AuthorId}_{request.ImageUrl.FileName}", Global.AuthorFolderName);

                if (imageUploadResponse.IsSuccessful)
                {
                    author.ImageUrl = imageUploadResponse.Url; // Update the author's image URL
                }
                else
                {
                    logger.LogWarning($"Failed to upload image for author {request.Name}. Error: {imageUploadResponse.Message}");
                }
            }

            // Update articles if needed
       //     author.Articles = request.Articles;

            // Save the updated author back to the repository
            await authorRepository.UpdateAuthorAsync(author);

            logger.LogInformation($"Successfully updated Author ID {request.AuthorId}.");
            return author.AuthorId;
        }
    }
}