using System.Net;
using MediatR;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video;

public static class BunnyDeleteVideoExtension
{
    public class BunnyDeleteResponse
    {
        public bool Success { get; set; }
    }


    public static async Task<BunnyDeleteResponse> DeleteVideo(this BunnyClient bunny, string videoId)
    {
        var libraryId = bunny.VideoLibraryId;
        var url = GetUrl(libraryId, videoId);
        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var httpRequest = new RestRequest("");
        var apiLibraryKey = bunny.VideoLibraryKey;
        var accessKey = bunny.ApiAccessKey;
        httpRequest.AddHeader("accept", "application/json");
        httpRequest.AddHeader(accessKey, apiLibraryKey);
        var response = await client.DeleteAsync(httpRequest);
        return new BunnyDeleteResponse { Success = response.StatusCode == HttpStatusCode.OK };
    }

    private static string GetUrl(string libraryId, string videoId)
    {
        return $"https://video.bunnycdn.com/library/{libraryId}/videos/{videoId}";
    }
}