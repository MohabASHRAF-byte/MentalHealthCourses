using AutoMapper;
using MentalHealthcare.Application.Podcast.Episodes.Commands.AddNewEpisode;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

public class PodcastProfile : Profile
{
    public PodcastProfile()
    {
        // Mapping from `AddNewEpisodeCommand` to `Podcast` (Fixes AutoMapper issue)
        CreateMap<AddNewEpisodeCommand, Podcast>()
            .ForMember(dest => dest.PodcastId, opt => opt.Ignore()) // ID is auto-generated
            .ForMember(dest => dest.AudioFile, opt => opt.Ignore()); // Set manually after BunnyCDN upload

        // Mapping from `Podcast` to `PodCastDto`
        CreateMap<Podcast, PodCastDto>()
            .ForMember(dest => dest.AudioFileUrl, opt => opt.MapFrom(src => src.AudioFile));
    }
}
