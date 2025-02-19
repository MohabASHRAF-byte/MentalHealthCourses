using Microsoft.AspNetCore.Http;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices;

public static class PurgeLink
{
    public static async Task ClearCacheAsync(this BunnyClient bunnyClient, string url, bool isAsync = true)
    {
        try
        {
            var encodedUrl = Uri.EscapeDataString(url);
            var requestLink = $"https://api.bunny.net/purge?url={encodedUrl}&async={isAsync.ToString().ToLower()}";

            var options = new RestClientOptions(requestLink);
            var client = new RestClient(options);

            var request = new RestRequest();
            request.AddHeader("AccessKey", bunnyClient.ApiAccessKey);

            var response = await client.PostAsync(request);

            if (!response.IsSuccessful)
            {
                throw new BadHttpRequestException(
                    string.Format(
                        "فشل في مسح ذاكرة التخزين المؤقت: {0} - {1}.",
                        response.StatusCode,
                        response.ErrorMessage
                    )
                );
            }
        }
        catch (Exception ex)
        {
            // Log the error properly
            Console.WriteLine($"Error clearing cache for URL {url}: {ex.Message}");
            throw;
        }
    }
}