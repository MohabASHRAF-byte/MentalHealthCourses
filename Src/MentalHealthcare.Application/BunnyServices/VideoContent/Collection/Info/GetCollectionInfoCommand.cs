using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Info;

public class GetCollectionInfoCommand:IRequest< string>
{
    public string LibraryId { get; set; }=default!;
    public string CollectionId { get; set; } = default!;
}