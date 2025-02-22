using AutoMapper;
using MentalHealthcare.Application.Author.Commands.Create;

namespace MentalHealthcare.Application.Author;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<CreateAuthorCommand, Domain.Entities.Author>()
            .ForMember(dest => dest.ImageUrl, 
                opt => opt.Ignore()) // Ignore fields set elsewhere
            .ForMember(dest => dest.Articles, 
                opt => opt.Ignore()); // Ignore related collections
    } 
}