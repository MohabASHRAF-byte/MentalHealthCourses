using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetAll;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Queries.GetAll
{
    public class GetAllAuthorsQueryHandler(
    ILogger<GetAllAuthorsQueryHandler> logger,
    IAuthorRepository auRepo,
    IMapper mapper,
    IUserContext userContext
    ) : IRequestHandler<GetAllAuthorsQuery, PageResult<AuthorDto>>
    {
        public async Task<PageResult<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetAllAuthorsQuery");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to get all Authors by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to get all Authors");
            }

            var Authors = await auRepo.GetAllAuthorsAsync(request.SearchText,
            request.PageNumber, request.PageSize );
            var AuDto = mapper.Map<IEnumerable<AuthorDto>>(Authors.Item2);

            return new PageResult<AuthorDto>(AuDto, Authors.Item1, request.PageSize, request.PageNumber);








        }
    }
}
