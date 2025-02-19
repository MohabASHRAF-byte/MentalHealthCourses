using System.Net.Http.Headers;
using MediatR;
using MentalHealthcare.Application.Resources.Localization.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.BunnyServices.Files.DeleteFile;

public class DeleteFileCommandHandler(
    IConfiguration configuration,
    ILocalizationService localizationService
    ):IRequestHandler<DeleteFileCommand>
{
    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var storageZoneName = configuration["BunnyCdn:StorageZoneName"]!;
        var accessKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
        var baseUrl = "storage.bunnycdn.com";
        
        var fileStorageUrl = $"https://{baseUrl}/{storageZoneName}/{request.Directory}/{request.FileName}";
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("AccessKey", accessKey);

        var response = await httpClient.DeleteAsync(fileStorageUrl, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

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