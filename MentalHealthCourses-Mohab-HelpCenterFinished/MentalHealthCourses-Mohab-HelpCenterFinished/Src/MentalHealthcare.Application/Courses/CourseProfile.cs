using AutoMapper;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Courses;

public class CourseProfile:Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourseCommand, Course>().ReverseMap();
        CreateMap<CourseMateriel, CourseMaterielDto>().ReverseMap();
        CreateMap<Course, CourseViewDto>().ReverseMap();
        CreateMap<InstructorDto, Instructor>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Course, CourseDto>().ReverseMap();
    }
}