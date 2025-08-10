using System.ComponentModel.DataAnnotations;

// Written By Marcelino, Reviewed by Mohab
// Reviewed
/*
   Review
   - add PodcastLength
   - podcast Description
 */
namespace MentalHealthcare.Domain.Entities;

public class Podcast : MaterialBe
{
    public int PodcastId { get; set; }
    public int EpisodeLength { get; set; }
    public string AudioFile { get; set; } = default!;
    public int PodcastSeriesId { get; set; } // Foreign Key property
    public PodcastSeries podcastSeries { get; set; } = default!;
}