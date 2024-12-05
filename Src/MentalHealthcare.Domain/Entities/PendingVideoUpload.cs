using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Domain.Entities;

public class PendingVideoUpload
{
    public string PendingVideoUploadId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CourseId { set; get; }
    public string Description { set; get; } = default!;
    public int AdminId { set; get; } = default!;
    public string Title { set; get; } = default!;

    public string Url { set; get; } = default!;

    //
    public int CourseSectionId { set; get; }
    public CourseSection Section { set; get; } = default!;
    public int CourseLessonId { set; get; }
    public CourseLesson Lesson { set; get; } = default!;
}