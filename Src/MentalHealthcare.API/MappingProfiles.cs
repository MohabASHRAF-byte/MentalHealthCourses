using AutoMapper;
using MentalHealthcare.Application;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Article, ArticlesDto>()
              .ForMember(A => A.UploadedBy, O => O.MapFrom(S => S.UploadedBy));
            CreateMap<Article, ArticlesDto>()
                                    .ForMember(A => A.AuthorinDto, O => O.MapFrom(S => S.Author));




            CreateMap<Meditation, MeditationDto>()
             .ForMember(A => A.UploadedByInDto, O => O.MapFrom(S => S.UploadedBy));
           
        }
    }
}
