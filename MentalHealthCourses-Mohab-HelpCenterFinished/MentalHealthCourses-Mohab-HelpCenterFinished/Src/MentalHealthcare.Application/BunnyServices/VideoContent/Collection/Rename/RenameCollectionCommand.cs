using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Rename;

public class RenameCollectionCommand : IRequest<bool>
{
    public string LibraryId { get; set; } = default!;
    public string CollectionId { get; set; } = default!;
    public string NewName { get; set; } = default!;
}