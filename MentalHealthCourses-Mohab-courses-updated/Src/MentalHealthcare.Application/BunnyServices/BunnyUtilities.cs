namespace MentalHealthcare.Application.BunnyServices;

public static class BunnyUtilities
{
    public static string GenerateVideoFrameUrl(this BunnyClient bunny, string videoId)
    {
        return $"https://iframe.mediadelivery.net/play/{bunny.VideoLibraryId}/{videoId}";
    }
}