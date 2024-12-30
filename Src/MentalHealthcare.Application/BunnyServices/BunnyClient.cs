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
    internal readonly string Region = configuration["BunnyCdn:Region"]!;
    internal readonly string BaseHostname = configuration["BunnyCdn:BaseHostname"]!;
    internal readonly string StorageZoneName = configuration["BunnyCdn:StorageZoneName"]!;
    internal readonly string AccessKey = configuration["BunnyCdn:StorageZoneAuthenticationKey"]!;
    internal readonly string HostName = configuration["BunnyCdn:Hostname"]!;
    internal readonly string ApiKey = configuration["BunnyCdn:ApiLibraryKey"]!;
    internal readonly string VideoLibraryId = configuration["BunnyCdn:LibraryId"]!;
    internal readonly string VideoLibraryKey = configuration["BunnyCdn:ApiLibraryKey"]!;
    internal readonly string ApiAccessKey = configuration["BunnyCdn:ApiAccessKey"]!;

    public CreateVideoCommandResponse GenerateSignature(string collectionId, string videoId)
    {
        var libraryId = VideoLibraryId;
        var expirationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600; // 1-hour expiration

        var signatureString = $"{libraryId}{ApiKey}{expirationTime}{videoId}";
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
    public async Task<UploadFileResponse> UploadFileAsync(IFormFile? file, string fileName, string folder)
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
        string hostname = string.IsNullOrEmpty(Region) ? BaseHostname : $"{Region}.{BaseHostname}";
        string url = $"https://{hostname}/{StorageZoneName}/{folder}/{filename}";
        string accessUrl = $"https://{HostName}/{folder}/{filename}";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.ContentType = "image/jpeg";
        request.Headers.Add("AccessKey", AccessKey);
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


    public async Task<DeleteFileResponse> DeleteFileAsync(string fileName, string folder = null)
    {
        try
        {
            // Build the URL
            var url = $"https://{BaseHostname}/{StorageZoneName}/";
            if (!string.IsNullOrEmpty(folder))
            {
                url += $"{folder.TrimEnd('/')}/"; // Ensure no trailing slash issues
            }
            url += $"{fileName}";
            var storageZoneName = StorageZoneName;
            var accessKey = AccessKey;
            
            
            // Create the RestClient and Request
            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest(url, Method.Delete); // Explicitly specify Method.Delete

            request.AddHeader("AccessKey", AccessKey);

            // Execute the request
            var response = await client.ExecuteAsync(request);

            // Check the response
            if (response.StatusCode == HttpStatusCode.OK) // Bunny API uses 204 for successful deletions
            {
                return new DeleteFileResponse
                {
                    IsSuccessful = true,
                    Message = "File deleted successfully"
                };
            }

            return new DeleteFileResponse
            {
                IsSuccessful = false,
                Message = $"Failed to delete file. Status: {response.StatusCode}, Error: {response.ErrorMessage}"
            };
        }
        catch (Exception e)
        {
            // Handle exceptions
            return new DeleteFileResponse
            {
                IsSuccessful = false,
                Message = $"Exception: {e.Message}"
            };
        }
    }
}