using MediatR;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.BunnyServices.UploadFile;

public class UploadFileCommandHandler(
    IConfiguration configuration
) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var contentType = "application/octet-stream";
        var filePath = $"{request.FilePath}/{request.FileName}";
        var storageZoneName = configuration["BunnyCdn:StorageZoneName"]!;
        var accessKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
        var baseUrl = "storage.bunnycdn.com";
        var url = $"https://{baseUrl}/{storageZoneName}/Photos/{request.FileName}";

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("AccessKey", accessKey);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        await using var fileStream = File.OpenRead(filePath);
        var content = new StreamContent(fileStream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        var response = await httpClient.PutAsync(url, content, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return url;
        }

        throw new Exception($"Error uploading file: {response.ReasonPhrase}");
    }
}

// Command class for the upload request
public class UploadFileCommand : IRequest<string>
{
    public string FilePath { get; set; } = default!;
    public string FileName { get; set; } = default!;

}