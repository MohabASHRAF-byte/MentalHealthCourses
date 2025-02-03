using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

// Written By Marcelino, Reviewed by Mohab
// Reviewed
/*
   Review
   - add PodcastLength
   - podcast Description
 */
namespace MentalHealthcare.Domain.Entities;
public class Podcast : MaterialBe
{   public int PodcastId { get; set; }
    public string File { get; set; } = default!; // File path or URL
    public int PodcastLength { get; set; }

    [MaxLength(1000)] public string? PodcastDescription { get; set; } = default!;
    [MaxLength(Global.UrlMaxLength)] public string? ThumbnailUrl { get; set; }
    [MaxLength(Global.TitleMaxLength)] public string? Title { get; set; } = default!;

    public int PodCasterId { get; set; } // Foreign Key property
    public PodCaster PodCaster { get; set; } = default!;
}