using AutoMapper;
using MentalHealthcare.Application.Courses.Course.Commands.Create;
using MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;
using MentalHealthcare.Application.Courses.Sections.Commands.Add_Section;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Dtos.course;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Courses;

public class CourseProfile:Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourseCommand, Domain.Entities.Courses.Course>().ReverseMap();
        CreateMap<Domain.Entities.Courses.Course, CourseViewDto>().ReverseMap();
        CreateMap<InstructorDto, Instructor>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<PendingVideoUpload, CourseLesson>().ReverseMap();
        CreateMap<CreateVideoCommand, PendingVideoUpload>();
        CreateMap<AddCourseSectionCommand, CourseSection>().ReverseMap();
        CreateMap<CourseLessonResource, CourseResourceDto>().ReverseMap();
        
        //
        CreateMap<Domain.Entities.Courses.Course, CourseDto>().ReverseMap();
        CreateMap<CourseSection, CourseSectionDto>().ReverseMap();
        CreateMap<CourseLesson, CourseLessonDto>().ReverseMap();
        CreateMap<CourseLesson, CourseLessonViewDto>().ReverseMap();
        CreateMap<Instructor, InstructorDto>().ReverseMap();
    }
}