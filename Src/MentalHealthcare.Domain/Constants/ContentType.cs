namespace MentalHealthcare.Domain.Constants;

public enum ContentType
{
    Video = 0,
    Image = 1,
    Audio = 2,
    Pdf = 3,
    Text = 4,
    Zip = 5
}

public static class ContentExtension
{
    public const string Video = ".mp4";
    public const string Image = ".jpeg";
    public const string Audio = ".mp3";
    public const string Pdf = ".pdf";
    public const string Text = ".txt";
    public const string Zip = ".zip";
}