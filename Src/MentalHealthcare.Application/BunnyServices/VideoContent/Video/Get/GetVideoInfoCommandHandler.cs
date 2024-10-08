using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Video.Get;

public class GetVideoInfoCommandHandler(
    IConfiguration configuration
) : IRequestHandler<GetVideoInfoCommand, VideoInfoDto?>
{
    public async Task<VideoInfoDto?> Handle(GetVideoInfoCommand request, CancellationToken cancellationToken)
    {
        var url = GetUrl(request.LibraryId, request.VideoId);
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
            var duration = ConversionUtility.ConvertSeconds(content.GetValue("length")!);
            var size = ConversionUtility.BitsToSizeString(content.GetValue("storageSize")!);
            var totalWatchTime = ConversionUtility.ConvertSeconds(content.GetValue("totalWatchTime")!);
            var avgWatchTime = ConversionUtility.ConvertSeconds(content.GetValue("averageWatchTime")!);
            var videoDto = new VideoInfoDto
            {
                VideoId = content.GetValue("guid")!,
                Title = content.GetValue("title")!,
                Views = content.GetValue("views")!,
                Duration = duration,
                StorageSize = size,
                TotalWatchTime = totalWatchTime,
                AverageWatchTime = avgWatchTime,
                Resolutions = content.GetValue("availableResolutions")!
            };
            return videoDto;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private static string GetUrl(string libraryId, string videoId)
    {
        return $"https://video.bunnycdn.com/library/{libraryId}/videos/{videoId}";
    }
}