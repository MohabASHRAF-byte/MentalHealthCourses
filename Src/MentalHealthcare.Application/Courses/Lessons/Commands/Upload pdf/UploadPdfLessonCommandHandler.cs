using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Upload_pdf;

public class UploadPdfLessonCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseLessonRepository courseLessonRepository,
    IConfiguration configuration,
    IMediator mediator,
    IUserContext userContext,
    IAdminRepository adminRepository) : IRequestHandler<UploadPdfLessonCommand,int>
{
    public async Task<int> Handle(UploadPdfLessonCommand request, CancellationToken cancellationToken)
    {
        //todo 
        //remove comment
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        //     throw new UnauthorizedAccessException();    }

        //todo
        // check for the size and other validations 
        var bunny = new BunnyClient(configuration);
        var courseName = await courseRepository.GetCourseName(request.CourseId);
        var lesson = new CourseLesson()
        {
            AdminId = 1,
            LessonName = request.PdfName,
            ContentType = ContentType.Pdf,
            CourseSectionId = request.SectionId,
            Url = "",
        };
        await courseLessonRepository.AddCourseLesson(lesson);
        lesson.LessonBunnyName = lesson.CourseLessonId + ContentExtension.Pdf;
        var uploadFileResponse = await bunny.UploadFileAsync(request.File, lesson.LessonBunnyName, courseName);
        lesson.Url = uploadFileResponse.Url!;
        await courseRepository.SaveChangesAsync();
        return lesson.CourseLessonId;
    }
}