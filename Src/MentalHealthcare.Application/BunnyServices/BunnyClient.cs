using System.Security.Cryptography;
using System.Text;
using MentalHealthcare.Application.Videos.Commands;
using MentalHealthcare.Application.Videos.Commands.CreateVideo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MentalHealthcare.Application.BunnyServices;

public class BunnyClient(
    IConfiguration configuration
    )
{
    // private readonly int libraryId = 326175;
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
}