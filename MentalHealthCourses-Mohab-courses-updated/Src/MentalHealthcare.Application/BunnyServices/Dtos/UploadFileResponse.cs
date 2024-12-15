namespace MentalHealthcare.Application.BunnyServices.Dtos;

public class UploadFileResponse
{
    public bool IsSuccessful { get; set; }
    public string? Message { get; set; }
    public string? Url { get; set; }
}