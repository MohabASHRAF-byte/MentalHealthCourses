using AutoMapper;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations
{
    public class MeditationProfile : Profile
    {
        public MeditationProfile()
        {// Mapping from Meditation entity to MeditationDto
            CreateMap<Meditation, MeditationDto>()
                .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom(src => src.UploadedBy.FName));
            // Optionally map MeditationDto back to Meditation
            CreateMap<MeditationDto, Meditation>()
                 .ForMember(dest => dest.UploadedBy, opt => opt.Ignore());
        }
    }
}

