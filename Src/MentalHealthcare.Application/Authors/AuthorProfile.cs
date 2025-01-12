using AutoMapper;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Application.Authors.Commands.Create;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors
{
    public class AuthorProfile : Profile

    {
        public AuthorProfile()
        {
            CreateMap<Domain.Entities.Author, AuthorDto>()
             .ForMember(dest => dest.Articles,
                 opt => opt.MapFrom(src => src.Articles.Select(T => T.Title).ToList()));

            // Optionally map AdvertisementDto back to Advertisement
            CreateMap<AuthorDto, Domain.Entities.Author>()
                .ForMember(dest => dest.Articles,
                    opt => opt.Ignore())
            .ForMember(dest => dest.AddedBy,
                    opt => opt.Ignore());// Ignoring AddedBy to handle separately

            CreateMap<CreateAuthorCommand, Domain.Entities.Author>().ReverseMap();

















        }




    }
}
