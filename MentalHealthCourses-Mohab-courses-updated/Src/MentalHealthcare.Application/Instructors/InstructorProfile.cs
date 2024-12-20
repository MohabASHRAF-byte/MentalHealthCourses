using AutoMapper;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Profiles
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            //TODO : Mapping from Instructor entity to InstructorDto
            CreateMap<Instructor, InstructorDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Select(c => new CourseViewDto
                {
                    Name = c.Name,
                    Price = c.Price,
                    Rating = c.Rating,
                    EnrollmentsCount = c.EnrollmentsCount
                }).ToList()));  //TODO :  Project courses to CourseViewDto list

            //TODO : If you need to map from InstructorDto back to Instructor (useful for updates), you can add a reverse map
            CreateMap<InstructorDto, Instructor>()
                .ForMember(dest => dest.Courses, opt => opt.Ignore());  //TODO : Typically courses would be handled separately
        }
    }
}
