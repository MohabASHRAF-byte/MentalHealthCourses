using MediatR;
using MentalHealthcare.Application.Common;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Rename;

public class RenameCollectionCommandHandler(
    IConfiguration configuration
) : IRequestHandler<RenameCollectionCommand, bool>
{
    public async Task<bool> Handle(RenameCollectionCommand request, CancellationToken cancellationToken)
    {
        var url = GetUrl(request.LibraryId, request.CollectionId);
        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var httpRequest = new RestRequest("");
        var apiLibraryKey = configuration["BunnyCdn:ApiLibraryKey"]!;
        var accessKey = configuration["BunnyCdn:AccessKey"]!;
        httpRequest.AddHeader("accept", "application/json");
        httpRequest.AddHeader(accessKey, apiLibraryKey);
        httpRequest.AddBody(new { name = request.NewName });
        var response = await client.PostAsync(httpRequest, cancellationToken);
        return response.IsSuccessful;
    }

    private string GetUrl(string libraryId, string collectionId)
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
   request.AddJsonBody("{\"name\":\"Mohab\"}", false);
   var response = await client.PostAsync(request);

   Console.WriteLine("{0}", response.Content);
   */