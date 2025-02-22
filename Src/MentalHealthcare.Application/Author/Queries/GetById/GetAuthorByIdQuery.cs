using MediatR;
using MentalHealthcare.Domain;
using MentalHealthcare.Domain.Dtos;

namespace MentalHealthcare.Application.Author.Queries.GetById;

public class GetAuthorByIdQuery :  IRequest<AuthorDto>
{
    public int AuthorID { get; set; }
}