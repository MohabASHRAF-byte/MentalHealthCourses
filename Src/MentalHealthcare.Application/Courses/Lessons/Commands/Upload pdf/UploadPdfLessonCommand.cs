using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.Lessons.Commands.Upload_pdf;

public class UploadPdfLessonCommand : IRequest<int>
{
    public string PdfName { get; set; }
    public int LessonLengthInSeconds { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]

    public IFormFile File { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; } = 0;

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; } = 0;

}