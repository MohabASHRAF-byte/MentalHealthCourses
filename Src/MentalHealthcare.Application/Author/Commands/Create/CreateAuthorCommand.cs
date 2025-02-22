using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentalHealthcare.Application.Author.Commands.Create;

public class CreateAuthorCommand : IRequest<CreateAuthorCommandResponse>
{
    
    
    public string Name { get; set; }
    public string? About { get; set; }
    public IFormFile File { get; set; } = default!;
    
}