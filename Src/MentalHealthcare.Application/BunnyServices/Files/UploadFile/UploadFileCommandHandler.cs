using System.Net.Http.Headers;
using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.BunnyServices.Files.UploadFile;

public class UploadFileCommandHandler(
    IConfiguration configuration,
    ILocalizationService localizationService
    ) : IRequestHandler<UploadFileCommand, string>
{
    private const string ContentType = "application/octet-stream";
    

    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var storageZoneName = configuration["BunnyCdn:StorageZoneName"]!;
        var pullZone = configuration["BunnyCdn:PullZone"]!;
        var accessKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
        var domain = configuration["BunnyCdn:Domain"]!;
        var baseUrl = "storage.bunnycdn.com"; 
        var fileStorageUrl = $"https://{baseUrl}/{storageZoneName}/{request.Directory}/{request.FileName}";
        var publicUrl = $"https://{pullZone}.{domain}/{request.Directory}/{request.FileName}";
        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("AccessKey", accessKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
       
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("AccessKey", accessKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // Read file content from IFormFile stream
        await using var fileStream = request.File.OpenReadStream();
        var content = new StreamContent(fileStream)
        {
            Headers = { ContentType = new MediaTypeHeaderValue(ContentType) }
        };

        // Upload file to BunnyCDN
        var response = await httpClient.PutAsync(fileStorageUrl, content, cancellationToken);

        // Handle response
        if (response.IsSuccessStatusCode)
        {
            var bunny = new BunnyClient(configuration);
            await bunny.ClearCacheAsync(publicUrl);
            return publicUrl;
        }

        // If the request fails, capture the error message
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new BadHttpRequestException(
            string.Format(
                localizationService.GetMessage("FileUploadError"),
                response.ReasonPhrase,
                errorContent
            )
        );
    }
}
