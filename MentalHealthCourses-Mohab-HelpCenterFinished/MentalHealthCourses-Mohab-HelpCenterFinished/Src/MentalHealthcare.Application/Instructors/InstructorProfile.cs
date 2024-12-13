using AutoMapper;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using System.Linq;

namespace MentalHealthcare.Application.Instructors
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            CreateMap<Instructor, InstructorDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Select(c => new CourseDto { CourseId = c.CourseId, Name = c.Name }).ToList()))
                .ForMember(d => d.AddedBy, o => o.MapFrom(s => new Admin { FName = s.AddedBy.FName, LName = s.AddedBy.LName }))
                .ReverseMap()
                .ForMember(dest => dest.Courses, opt => opt.Ignore())
                .ForMember(dest => dest.AddedBy, opt => opt.Ignore());
        }
    }
}
