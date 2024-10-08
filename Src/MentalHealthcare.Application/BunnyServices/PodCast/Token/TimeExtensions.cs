namespace MentalHealthcare.Application.BunnyServices.PodCast.Token
{
    internal static class TimeExtensions
    {
        internal static string ToUnixTimestamp(this DateTimeOffset time)
            => time.ToUnixTimeSeconds().ToString();
    }


}
