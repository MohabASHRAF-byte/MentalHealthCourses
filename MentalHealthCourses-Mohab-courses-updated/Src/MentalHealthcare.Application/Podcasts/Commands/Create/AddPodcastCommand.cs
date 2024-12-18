using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Podcasts.Commands.Create
{
    public class AddPodcastCommand : IRequest<int>
    {
    
        public string Title { get; set; }
        public string Description { get; set; }  // Added description
        public IFormFile Url { get; set; }  // File for the podcast
        public int UploadedById { get; set; } // ID of the admin or uploader
        public int PodCasterId { get; set; } // ID of the podcaster
        public int PodcastLength { get; set; } // Length of the podcast in seconds
    }
}
