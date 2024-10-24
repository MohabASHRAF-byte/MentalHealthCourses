using MediatR;

namespace MentalHealthcare.Application.BunnyServices.VideoContent.Collection.Add;

public class AddCollectionCommand : IRequest<string>
{
    public string CollectionName { get; set; } = default!;
}