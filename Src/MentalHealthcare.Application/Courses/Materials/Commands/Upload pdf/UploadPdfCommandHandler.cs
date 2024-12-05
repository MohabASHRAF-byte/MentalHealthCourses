using MediatR;
using MentalHealthcare.Application.BunnyServices;
using MentalHealthcare.Application.Courses.Materials.Commands.CreateVideo;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Courses.Materials.Commands.Upload_pdf;

public class UploadPdfCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    IConfiguration configuration,
    IMediator mediator,
    IUserContext userContext,
    IAdminRepository adminRepository) : IRequestHandler<UploadPdfCommand>
{
    public async Task Handle(UploadPdfCommand request, CancellationToken cancellationToken)
    {
        //todo 
        //remove comment
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        //     throw new UnauthorizedAccessException();    }

        //todo
        // check for the size and other validations 
        var bunny = new BunnyClient(configuration);
        var course = await courseRepository.GetByIdAsync(request.CourseId);
        var material = new CourseMateriel()
        {
            CourseId = request.CourseId,
            Description = request.Description,
            AdminId = 1,
            Title = request.PdfName,
            IsVideo = false,
            CourseSectionId = request.SectionId,
            CourseLessonId = request.LessonId,
            Url = ""
        };
        await courseRepository.AddCourseMatrial(material);
        var name = material.CourseMaterielId.ToString() + ".pdf";
        var uploadFileResponse = await bunny.UploadFile(request.File, name, course.Name);
        material.Url = uploadFileResponse.Url;
        await courseRepository.SaveChangesAsync();
    }
}