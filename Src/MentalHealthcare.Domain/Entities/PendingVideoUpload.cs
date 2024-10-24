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
}