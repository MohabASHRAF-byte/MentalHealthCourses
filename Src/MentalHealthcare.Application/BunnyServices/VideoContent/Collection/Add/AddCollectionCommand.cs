using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;

public class AddCollectionCommand : IRequest<string>
{
    public string LibraryId { get; set; } = default!;
    public string CollectionName { get; set; } = default!;
}