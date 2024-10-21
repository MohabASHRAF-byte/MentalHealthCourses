using System.ComponentModel.DataAnnotations;

// Written By Marcelino, Reviewed by Mohab
// Reviewed
/*
   Review
   - add PodcastLength
   - podcast Description
 */
namespace MentalHealthcare.Domain.Entities;

public class Podcast : MaterialBE
{
    public int PodcastId { get; set; }

    public int PodcastLength { get; set; }

    [MaxLength(1000)] public string PodcastDescription { get; set; } = default!;

    public int PodCasterId { get; set; } // Foreign Key property
    public PodCaster PodCaster { get; set; } = default!;
}