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

/// <summary>
/// Command handler for uploading a PDF lesson.
/// </summary>
public class UploadPdfLessonCommandHandler(
    ILogger<CreateVideoCommandHandler> logger,
    ICourseRepository courseRepository,
    ICourseLessonRepository courseLessonRepository,
    IConfiguration configuration,
    IMediator mediator,
    IUserContext userContext,
    IAdminRepository adminRepository) : IRequestHandler<UploadPdfLessonCommand, int>
{
    public async Task<int> Handle(UploadPdfLessonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting UploadPdfLessonCommandHandler for CourseId: {CourseId}, SectionId: {SectionId}, PdfName: {PdfName}",
            request.CourseId, request.SectionId, request.PdfName);

        // TODO: Uncomment and implement user validation.
        // var currentUser = userContext.GetCurrentUser();
        // if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
        // {
        //     logger.LogError("Unauthorized access attempt for uploading PDF.");
        //     throw new UnauthorizedAccessException();
        // }

        var fileSizeInMB = request.File.Length / (1 << 20); // Convert bytes to MB
        if (fileSizeInMB > Global.CourseLessonPdfSize)
        {
            logger.LogError("File size ({FileSize}MB) exceeds the limit of {Limit}MB.", fileSizeInMB,
                Global.CourseRecourseSize);
            throw new ArgumentException($"The file {request.PdfName} is too large.");
        }

        logger.LogInformation("Fetching course name for CourseId: {CourseId}", request.CourseId);
        var courseName = await courseRepository.GetCourseName(request.CourseId);

        logger.LogInformation("Initializing new CourseLesson entity for PDF upload.");
        var lesson = new CourseLesson
        {
            AdminId = 1, // TODO: Replace hardcoded AdminId with the current user's ID when user context is implemented.
            LessonName = request.PdfName,
            ContentType = ContentType.Pdf,
            CourseSectionId = request.SectionId,
            Url = string.Empty // URL will be updated after uploading the file.
        };

        logger.LogInformation("Adding new CourseLesson to the repository.");
        await courseLessonRepository.AddCourseLesson(lesson);

        lesson.LessonBunnyName = $"{lesson.CourseLessonId}{ContentExtension.Pdf}";

        logger.LogInformation("Uploading PDF to BunnyCDN with name: {LessonBunnyName}", lesson.LessonBunnyName);
        var bunny = new BunnyClient(configuration);
        var uploadFileResponse = await bunny.UploadFileAsync(request.File, lesson.LessonBunnyName, courseName);

        if (uploadFileResponse.Url == null)
        {
            logger.LogError("Failed to upload PDF to BunnyCDN for LessonId: {CourseLessonId}", lesson.CourseLessonId);
            throw new Exception("Failed to upload PDF. Try again.");
        }

        lesson.Url = uploadFileResponse.Url;
        logger.LogInformation("Successfully uploaded PDF. Updating CourseLesson with URL: {Url}", lesson.Url);

        await courseRepository.SaveChangesAsync();
        logger.LogInformation(
            "UploadPdfLessonCommandHandler successfully completed for CourseLessonId: {CourseLessonId}",
            lesson.CourseLessonId);

        return lesson.CourseLessonId;
    }
}