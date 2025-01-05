using MentalHealthcare.Application.Common;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video;

public static class CreateBunnyVideo
{
    public static async Task<string?> CreateVideoAsync(this BunnyClient bunny ,string videoName , string collectionId)
    {

        var url = GetUrl(bunny.VideoLibraryId);
        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var httpRequest = new RestRequest("");
        var apiLibraryKey = bunny.ApiKey;
        var accessKey =bunny.ApiAccessKey;
        httpRequest.AddHeader("accept", "application/json");
        httpRequest.AddHeader("AccessKey", apiLibraryKey);
        httpRequest.AddBody(new
        {
            title = videoName,
            collectionId = collectionId
        });
        
        var response = await client.PostAsync(httpRequest);
        var content = new JsonHelper(response);
        try
        {
            var videoId = content.GetValue<string>("guid");
            return videoId!;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string GetUrl(string libraryId)
    {
        return $"https://video.bunnycdn.com/library/{libraryId}/videos";
    }
}
