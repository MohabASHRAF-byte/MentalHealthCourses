using AutoMapper;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Application.Courses.Lessons.Commands;
using MentalHealthcare.Application.Courses.Lessons.Commands.Add_Lesson;
using MentalHealthcare.Application.Courses.Materials.Commands.ConfirmUpload;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Courses;

public class CourseProfile:Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourseCommand, Course>().ReverseMap();
        CreateMap<Course, CourseViewDto>().ReverseMap();
        CreateMap<InstructorDto, Instructor>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<AddLessonCommand, CourseLesson>().ReverseMap();
        CreateMap<PendingVideoUpload, CourseMateriel>().ReverseMap();
        //
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<CourseSection, CourseSectionDto>().ReverseMap();
        CreateMap<CourseLesson, CourseLessonDto>().ReverseMap();
        CreateMap<CourseMateriel, CourseMaterielDto>().ReverseMap();
        CreateMap<Instructor, InstructorDto>().ReverseMap();
    }
}