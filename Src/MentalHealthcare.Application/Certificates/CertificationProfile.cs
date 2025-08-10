using AutoMapper;
using MentalHealthcare.Application.Certificates.Commands.Generate;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Certificates
{
    public class CertificationProfile : Profile
    {


        public CertificationProfile()
        {
            CreateMap<GenerateCertificateCommand, Certificate>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.userEmail))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.userName))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.courseName))
                .ForMember(dest => dest.DateGenerated, opt => opt.MapFrom(src => DateTime.UtcNow));
        }







    }
}
