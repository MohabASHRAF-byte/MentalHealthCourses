using RestSharp;

namespace MentalHealthcare.Application.BunnyServices;

public static class PurgeLink
{
    public static async Task ClearCacheAsync(this BunnyClient bunnyClient, string url, bool async = true)
    {
        var requestLink = $"https://api.bunny.net/purge?url={url}&async={async}";
        var options = new RestClientOptions(requestLink);
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("AccessKey", bunnyClient.AccessKey);

        await client.PostAsync(request);
    }
}