using AutoMapper;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems
{
    public class GetHomeScreenItemsProfile : Profile
    {

        public GetHomeScreenItemsProfile()
        {
            // Map Advertisement -> AdvertisementDto ✅
            CreateMap<MentalHealthcare.Domain.Entities.Advertisement, AdvertisementDto>()
                .ForMember(dest => dest.AdvertisementImageUrls, opt => opt.MapFrom(src => src.AdvertisementImageUrls.Select(img => img.ImageUrl).ToList()));

            // Map Instructor -> InstructorDto ✅
            CreateMap<Instructor, InstructorDto>()
                .ForMember(dest => dest.RecentlyAddedCourses, opt => opt.MapFrom(src => src.Courses));

            // Map Course -> CourseViewDto ✅
            CreateMap<Course, CourseViewDto>();
        }







    }
}
