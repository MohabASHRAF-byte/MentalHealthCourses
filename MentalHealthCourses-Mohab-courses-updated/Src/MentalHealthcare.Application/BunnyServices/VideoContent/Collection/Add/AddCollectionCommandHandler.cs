using MentalHealthcare.Application.Common;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;

public static class MakeNewVideoCollection
{
    public static async Task<string?> CreateVideoFolderAsync(this BunnyClient bunnyClient, string collectionName)
    {
        var libraryId = bunnyClient.VideoLibraryId;
        var url = GetUrl(libraryId);
        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var httpRequest = new RestRequest("");
        var apiLibraryKey = bunnyClient.VideoLibraryKey;
        httpRequest.AddHeader("accept", "application/json");
        httpRequest.AddHeader("AccessKey", apiLibraryKey);
        httpRequest.AddBody(new { name = collectionName });
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

    private static string GetUrl(string requestLibraryId)
    {
        return $"https://video.bunnycdn.com/library/{requestLibraryId}/collections";
    }
}