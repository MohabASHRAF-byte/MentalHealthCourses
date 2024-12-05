using System.Net;
using System.Security.Cryptography;
using System.Text;
using MentalHealthcare.Application.BunnyServices.Dtos;
using MentalHealthcare.Application.Videos.Commands;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices;

public class BunnyClient(
    IConfiguration configuration
)
{
    private readonly string _region = configuration["BunnyCdn:Region"]!; 
    private readonly string _baseHostname = configuration["BunnyCdn:BaseHostname"]!;
    private readonly string _storageZoneName = configuration["BunnyCdn:StorageZoneName"]!;
    private readonly string _accessKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
    private readonly string _hostName = configuration["BunnyCdn:Hostname"]!;
    private readonly string _apiKey = configuration["BunnyCdn:ApiLibraryKey"]!;

    public CreateVideoCommandResponse GenerateSignature(string libraryId, string collectionId, string videoId)
    {
        var expirationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600; // 1-hour expiration

        var signatureString = $"{libraryId}{_apiKey}{expirationTime}{videoId}";
        var signature = GenerateSha256Signature(signatureString);

        var response = new CreateVideoCommandResponse
        {
            AuthorizationSignature = signature,
            AuthorizationExpire = expirationTime.ToString(),
            VideoId = videoId,
            LibraryId = libraryId,
            CollectionId = collectionId
        };
        return response;
    }

    private static string GenerateSha256Signature(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    // 1 - check if the file is empty 
    // 2 -
    public async Task<UploadFileResponse> UploadFile(IFormFile? file, string fileName, string folder)
    {
        if (file == null || file.Length == 0)
        {
            return new UploadFileResponse()
            {
                IsSuccessful = false,
                Message = "File is empty"
            };
        }

        var filename = fileName;
        string hostname = string.IsNullOrEmpty(_region) ? _baseHostname : $"{_region}.{_baseHostname}";
        string url = $"https://{hostname}/{_storageZoneName}/{folder}/{filename}";
        string accessUrl = $"https://{_hostName}/{folder}/{filename}";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.ContentType = "image/jpeg";
        request.Headers.Add("AccessKey", _accessKey);
        try
        {
            using (Stream fileStream = file.OpenReadStream())
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await fileStream.CopyToAsync(requestStream);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                {
                    return new UploadFileResponse()
                    {
                        IsSuccessful = true,
                        Message = "File uploaded successfully",
                        Url = accessUrl
                    };
                }

                return new UploadFileResponse()
                {
                    IsSuccessful = false,
                    Message = "Failed to upload file "
                };
            }
        }
        catch (Exception ex)
        {
            return new UploadFileResponse()
            {
                IsSuccessful = false,
                Message = ex.Message
            };
        }
    }

    public async Task<DeleteFileResponse> DeleteFile(string fileName, string folder = null)
    {
        string url = $"https://{_baseHostname}/{_storageZoneName}/";
        if (!string.IsNullOrEmpty(folder))
        {
            url += $"{folder}/";
        }

        url += $"{fileName}";

        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("AccessKey", _accessKey);
        try
        {
            var response = await client.DeleteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new DeleteFileResponse()
                {
                    IsSuccessful = true,
                    Message = "File deleted successfully"
                };
            }

            return new DeleteFileResponse()
            {
                IsSuccessful = false,
                Message = "Failed to delete file "
            };
        }
        catch (Exception e)
        {
            return new DeleteFileResponse()
            {
                IsSuccessful = false,
                Message = e.Message
            };
        }
    }
}