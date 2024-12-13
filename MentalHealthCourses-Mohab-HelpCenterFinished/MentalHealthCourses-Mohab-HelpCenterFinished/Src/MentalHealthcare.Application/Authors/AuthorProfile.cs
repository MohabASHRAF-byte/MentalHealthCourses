using AutoMapper;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors
{
    public class AuthorProfile : Profile
    {public AuthorProfile()
    {CreateMap<Author, AuthorDto>()
    .ForMember(d => d.AddedBy, O => O.MapFrom(s => s.AddedBy.FName))
    .ForMember(d => d.Articles , O => O.MapFrom(s => s.Articles.Select(T => T.Title).ToList()))
     .ReverseMap().ForMember(dest => dest.Articles, opt => opt.Ignore()); } }
}
