using System.Text.Json.Serialization;
using MediatR;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Courses.LessonResources.Commands.Upload_Resource;

public class UploadLessonResourceCommand: IRequest<int>
{
    [JsonIgnore]
    public int CourseId { get; set; }
    [JsonIgnore]
    public int SectionId { get; set; }
    [JsonIgnore]
    public int LessonId { get; set; }
    
    public IFormFile File { get; set; }
    
    public string FileName { get; set; }
    
    public ContentType ContentType { get; set; }
}