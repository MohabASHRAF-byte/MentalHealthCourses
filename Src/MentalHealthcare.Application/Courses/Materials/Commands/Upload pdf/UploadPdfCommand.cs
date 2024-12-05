using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.Materials.Commands.Upload_pdf;

public class UploadPdfCommand : IRequest
{
    public string PdfName { get; set; }
    [MaxLength(500)] public string? Description { set; get; }
    [System.Text.Json.Serialization.JsonIgnore]

    public IFormFile File { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]

    public int CourseId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int SectionId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]

    public int LessonId { get; set; }
}