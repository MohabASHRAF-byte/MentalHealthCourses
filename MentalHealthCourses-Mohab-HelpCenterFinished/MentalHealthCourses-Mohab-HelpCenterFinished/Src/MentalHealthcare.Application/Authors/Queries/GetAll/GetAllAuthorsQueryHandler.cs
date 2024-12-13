using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articles.Queries.GetAll;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Queries.GetAll
{
    public class GetAllAuthorsQueryHandler(
     ILogger<GetAllAuthorsQueryHandler> logger,
    IMapper mapper,
    IAuthorRepository authorRepository) : IRequestHandler<GetAllAuthorsQuery, PageResult<AuthorDto>>
    {
        public async Task<PageResult<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {




            // TODO: add auth
            logger.LogInformation("Retrieving all Authors with search text: {SearchText}, page number: {PageNumber}, page size: {PageSize}", request.SearchText, request.PageNumber, request.PageSize);


            // Retrieve all Articles from the repository
            var Authors = await authorRepository.GetAllAuthors(request.SearchText, request.PageNumber, request.PageSize);

            // Log the number of Articles retrieved
            logger.LogInformation("Retrieved {Count} Author.", Authors.Item1);

            // Map the retrieved Articles to DTOs
            var AuthorDtos = mapper.Map<IEnumerable<AuthorDto>>(Authors.Item2);

            // Create the page result
            var count = Authors.Item1;
            var ret = new PageResult<AuthorDto>(AuthorDtos, count, request.PageSize, request.PageNumber);

            return ret;
















        }
    }
}
