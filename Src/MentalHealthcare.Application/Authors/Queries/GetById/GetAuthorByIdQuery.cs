using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Queries.GetById
{
    public class GetAuthorByIdQuery : IRequest<AuthorDto>
    {
        public int AuthorId { get; set; }
    }
}
