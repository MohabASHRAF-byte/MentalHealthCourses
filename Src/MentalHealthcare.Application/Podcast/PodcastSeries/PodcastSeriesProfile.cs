using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries
{
    public class PodcastSeriesProfile : Profile
    {



        public PodcastSeriesProfile()
        {
            // Mapping for creating a new Podcast Series
            CreateMap<AddPodcastSeriesCommand, MentalHealthcare.Domain.Entities.PodcastSeries>();

            // Mapping for retrieving Podcast Series
            CreateMap<MentalHealthcare.Domain.Entities.PodcastSeries, PodcastSeriesDto>()
                .ForMember(dest => dest.Podcasts, opt => opt.MapFrom(src => src.Podcasts ?? new List<MentalHealthcare.Domain.Entities.Podcast>()));

            // Ensure Podcasts are mapped properly
            CreateMap<MentalHealthcare.Domain.Entities.Podcast, PodCastDto>()
                .ForMember(dest => dest.AudioFileUrl, opt => opt.MapFrom(src => src.AudioFile));

            // 🔹 Ensure Reverse Mapping (optional but useful)
            CreateMap<PodCastDto, MentalHealthcare.Domain.Entities.Podcast>();
        }





    }
}
