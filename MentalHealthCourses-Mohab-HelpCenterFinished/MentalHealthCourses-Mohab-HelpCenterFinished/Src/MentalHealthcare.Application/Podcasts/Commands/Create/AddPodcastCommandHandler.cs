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

namespace MentalHealthcare.Application.Podcasts.Commands.Create
{
    public class AddPodcastCommandHandler(
    ILogger<AddPodcastCommandHandler> logger,
    IPodcastRepository PodRepo,
    IConfiguration configuration,
    IMapper mapper
) : IRequestHandler<AddPodcastCommand, int>
    {
        public async Task<int> Handle(AddPodcastCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(@"Creating Podcast with name {ad}", request.Name);
            //todo 
            // add auth


            CheckRecordSize(ref request);

            var NewPodcast = mapper.Map<Domain.Entities.Podcast>(request);
             var bunny = new BunnyClient(configuration);

            


                           //Upload The Podcast
            foreach (var PodCast in request.PodcastDescription)
            {
                var NewAudioFile = $"{NewPodcast.PodcastId}.m4a";  //or  MP3 but m4a is better in quality

                // Upload the Audio File to BunnyCDN
                var response = await bunny.UploadFile(PodCast, NewAudioFile, Global.PodCasterFolderName);

                if (!response.IsSuccessful || response.Url == null)
                {
                    logger.LogWarning(@"Could not upload Podcast {ad} error msg :{mg}", request.Name,
                        response.Message ?? ""
                    );
                    continue;
                }

                NewPodcast.PodcastDescription = response.Url;

            }



            await PodRepo.AddPodcastAsync(NewPodcast);

            return NewPodcast.PodcastId;
        }





        private void CheckRecordSize(ref AddPodcastCommand request)
        {
            foreach (var Audio in request.PodcastDescription)
            {
                var PodcastSizeInMb = Audio.Length / (1 << 20);
                if (PodcastSizeInMb > Global.PodCasterImgSize)
                {
                    logger.LogWarning($"try to upload Audio with size {PodcastSizeInMb} ");
                    throw new Exception($"Image size cannot be greater than {Global.PodCasterImgSize} MB");
                }
            }
        }



    }
    }

