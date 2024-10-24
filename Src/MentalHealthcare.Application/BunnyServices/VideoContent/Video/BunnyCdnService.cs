using System;
using System.Security.Cryptography;
using System.Text;

public class BunnyCdnService
{
    private readonly string _storageZoneName;
    private readonly string _accessKey;
    private readonly string _domain;

    public BunnyCdnService(string storageZoneName, string accessKey, string domain)
    {
        _storageZoneName = storageZoneName;
        _accessKey = accessKey;
        _domain = domain;
    }

    public string GenerateSignedUrl(string fileName, string directory, TimeSpan expiration)
    {
        var baseUrl = $"https://storage.bunnycdn.com/{_storageZoneName}/{directory}/{fileName}";
        var expirationTimestamp = DateTime.UtcNow.Add(expiration).ToString("yyyy-MM-ddTHH:mm:ss");

        // Generate the signature using HMACSHA256
        string stringToSign = $"{baseUrl}{expirationTimestamp}";
        var encoding = new ASCIIEncoding();
        byte[] keyBytes = encoding.GetBytes(_accessKey);
        byte[] messageBytes = encoding.GetBytes(stringToSign);

        using var hmacsha256 = new HMACSHA256(keyBytes);
        byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
        var signature = Convert.ToBase64String(hashmessage);

        // Return the signed URL with expiration and signature
        return $"{baseUrl}?expires={expirationTimestamp}&token={signature}";
    }
}