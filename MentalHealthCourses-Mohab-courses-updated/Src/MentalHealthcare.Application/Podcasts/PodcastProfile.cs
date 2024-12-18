using AutoMapper;
using MentalHealthcare.Application.Podcasts.Commands.Create;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Podcasts.Profiles
{
    public class PodcastProfile : Profile
    {
        public PodcastProfile()
        {
            // Map AddPodcastCommand to Domain.Entities.Podcast
            CreateMap<AddPodcastCommand, Podcast>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.PodcastDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url.FileName))  // Assuming `Url` is a file uploaded via IFormFile
                .ForMember(dest => dest.PodcastId, opt => opt.MapFrom(src => src.PodCasterId))
                .ForMember(dest => dest.UploadedById, opt => opt.MapFrom(src => src.UploadedById))
                .ForMember(dest => dest.PodcastLength, opt => opt.MapFrom(src => src.PodcastLength));

            // Map Domain.Entities.Podcast to PodCastDto
            CreateMap<Podcast, PodCastDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));  // Assuming `Url` is a string in `Podcast` and `PodCastDto`
        }
    }
}
