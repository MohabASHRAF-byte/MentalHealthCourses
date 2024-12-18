using AutoMapper;
using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Podcasts.Commands.Create;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcasts.Commands.Create
{
    public class AddPodcastCommandHandler : IRequestHandler<AddPodcastCommand, int>
    {
        private readonly ILogger<AddPodcastCommandHandler> _logger;
        private readonly IPodCastRepository _podRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AddPodcastCommandHandler(
            ILogger<AddPodcastCommandHandler> logger,
            IPodCastRepository podRepo, 
            IConfiguration configuration,
            IMapper mapper)
        {
            _logger = logger;
            _podRepo = podRepo;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddPodcastCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating Podcast with title {Title}", request.Title);

            // Check podcast size
            CheckRecordSize(ref request);

            // Map the incoming command to a domain entity
            var newPodcast = _mapper.Map<Domain.Entities.Podcast>(request);
            var bunny = new BunnyClient(_configuration);

            var newAudioFile = $"{newPodcast.PodcastId}.m4a";  // or MP3, but M4A is better quality

            // Upload the audio file to BunnyCDN
            var response = await bunny.UploadFileAsync(request.Url, newAudioFile, Global.PodCasterFolderName);

            if (!response.IsSuccessful || response.Url == null)
            {
                _logger.LogWarning("Could not upload Podcast {Title}. Error: {ErrorMessage}",
                    request.Title, response.Message ?? "Unknown error");
                throw new Exception($"Failed to upload podcast {request.Title} to BunnyCDN.");
            }

            // Set the URL of the podcast in the domain entity
            newPodcast.Url = response.Url;

            // Add the podcast to the repository
            await _podRepo.CreateAsync(newPodcast);

            // Return the new podcast's ID
            return newPodcast.PodcastId;
        }

        private void CheckRecordSize(ref AddPodcastCommand request)
        {
            var podcastSizeInMb = request.PodcastLength / (1 << 20);
            if (podcastSizeInMb > Global.PodCasterImgSize)
            {
                _logger.LogWarning("Attempting to upload audio file with size {SizeInMb} MB", podcastSizeInMb);
                throw new Exception($"Podcast size cannot be greater than {Global.PodCasterImgSize} MB");
            }
        }
    }
}
