namespace MentalHealthcare.Domain.Dtos;

public class PodCastDto
{
    public int PodcastId { get; set; }
    public string Title { get; set; } = default!;
    public string AudioFileUrl { get; set; } = default!; // URL for the uploaded audio file
    public int EpisodeLength { get; set; } // Length in seconds or minutes
    public int PodcastSeriesId { get; set; } // Reference to the Podcast Series
}