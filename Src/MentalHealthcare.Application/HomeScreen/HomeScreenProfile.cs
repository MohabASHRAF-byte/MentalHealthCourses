using AngleSharp.Dom;
using AutoMapper;
using MentalHealthcare.Application.Instructors.Commands.Create;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreen
{
    public class HomeScreenProfile : Profile
    {

        public HomeScreenProfile()
        {
            // Advertisement Mapping
            CreateMap<Domain.Entities.Advertisement, AdvertisementDto>()
                .ForMember(dest => dest.AdvertisementImageUrls, opt => opt.MapFrom(src => src.AdvertisementImageUrls))
                .ReverseMap();



            CreateMap<CreateInstructorCommand, Instructor>()
             .ForMember(dest => dest.ImageUrl,
          opt => opt.Ignore()) // Ignore fields set elsewhere
            .ForMember(dest => dest.Courses,
            opt => opt.Ignore()); // Ignore related collections


            // Course Mapping (Ensuring Proper Course Mapping)
            CreateMap<Course, CourseDto>().ReverseMap();

        }











    }
}
