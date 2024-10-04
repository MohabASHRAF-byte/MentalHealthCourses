using MediatR;
using MentalHealthcare.Application.Common;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Info;

public class GetCollectionInfoCommandHandler(
    IConfiguration configuration
    ):IRequestHandler<GetCollectionInfoCommand, string>
{
    public async Task<string> Handle(GetCollectionInfoCommand request, CancellationToken cancellationToken)
    {
        var url = GetUrl(request.LibraryId,request.CollectionId);
        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var httpRequest = new RestRequest("");
        var apiLibraryKey = configuration["BunnyCdn:ApiLibraryKey"]!;
        var accessKey = configuration["BunnyCdn:AccessKey"]!;
        httpRequest.AddHeader("accept", "application/json");
        httpRequest.AddHeader(accessKey, apiLibraryKey);
        var response = await client.GetAsync(httpRequest, cancellationToken);
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

    private string GetUrl(string libraryId,string collectionId)
    {
        return $"https://video.bunnycdn.com/library/{libraryId}/collections/{collectionId}";
    }
}
/*
   var options = new RestClientOptions("https://video.bunnycdn.com/library/317728/collections/3dd71c10-155e-4dd5-9cbc-edc9b746827d");
   var client = new RestClient(options);
   var request = new RestRequest("");
   request.AddHeader("accept", "application/json");
   request.AddHeader("AccessKey", "709aef68-e4a2-4587-b27d90b0fc7b-7ac0-4fc3");
   var response = await client.GetAsync(request);
   
   Console.WriteLine("{0}", response.Content);
   */