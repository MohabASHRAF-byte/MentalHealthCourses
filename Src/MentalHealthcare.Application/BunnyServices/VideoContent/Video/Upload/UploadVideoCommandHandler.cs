using MediatR;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.Upload;

public class UploadVideoCommandHandler(
    IConfiguration configuration
) : IRequestHandler<UploadVideoCommand, bool>
{
    public async Task<bool> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        var options = new RestClientOptions(GetUrl(request.LibraryName, request.VideoId));
        var client = new RestClient(options);
        var uploadRequest = new RestRequest();
        var apiLibraryKey = configuration["BunnyCdn:ApiLibraryKey"]!;
        var accessKey = configuration["BunnyCdn:AccessKey"]!;
        uploadRequest.AddHeader(accessKey, apiLibraryKey);
        uploadRequest.AddFile("file.mp4", request.FilePath);
        var response = await client.PutAsync(uploadRequest, cancellationToken: cancellationToken);
        return response.IsSuccessful;
    }

    private string GetUrl(string libraryName, string videoId)
    {
        return $"https://video.bunnycdn.com/library/{libraryName}/videos/{videoId}";
    }
}