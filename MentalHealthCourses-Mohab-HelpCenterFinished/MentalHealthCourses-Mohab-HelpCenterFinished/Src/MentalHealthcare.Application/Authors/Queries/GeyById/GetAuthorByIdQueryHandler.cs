using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Queries.GeyById
{
    public class GetAuthorByIdQueryHandler(
    ILogger<GetAuthorByIdQueryHandler> logger,
    IAuthorRepository authorRepository,
    IMapper mapper
    ) : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
    {
        public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {



            logger.LogInformation($"GetAuthorByIdQueryHandler invoked.");
            logger.LogInformation($"GetAuthorByIdQueryHandler. Request: {request.AuthorId}");
            var ad = await authorRepository.GetAuthorById(request.AuthorId);
            var AuthorDto = mapper.Map<AuthorDto>(ad);
            return AuthorDto;










        }
    }
}
