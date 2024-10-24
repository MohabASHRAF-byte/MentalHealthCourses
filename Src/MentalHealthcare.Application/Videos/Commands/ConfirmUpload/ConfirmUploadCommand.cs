using MediatR;

namespace MentalHealthcare.Application.Videos.Commands.ConfirmUpload;

public class ConfirmUploadCommand:IRequest
{
    public string videoId { get; set; }
    public bool Confirmed { get; set; }
}