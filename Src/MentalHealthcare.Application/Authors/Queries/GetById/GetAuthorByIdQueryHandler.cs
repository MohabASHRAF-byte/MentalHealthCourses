using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Queries.GetById;
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

namespace MentalHealthcare.Application.Authors.Queries.GetById
{
    public class GetAuthorByIdQueryHandler(
    ILogger<GetAuthorByIdQueryHandler> logger,
    IAuthorRepository AuRepository,
    IMapper mapper,
    IUserContext userContext
    ) : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
    {
        public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation($"GetAuthorByIdQueryHandler invoked.");
            logger.LogInformation($"GetAuthorByIdQueryHandler. Request: {request.AuthorId}");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to get Author by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to get Author");
            }
            var AU = await AuRepository.GetAuthorById(request.AuthorId);
            var auDto = mapper.Map<AuthorDto>(AU);
            return auDto;








        }
    }
}
