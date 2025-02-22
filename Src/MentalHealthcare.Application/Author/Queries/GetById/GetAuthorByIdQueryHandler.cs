using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Author.Queries.GetById;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Author.Queries.GetById
{
    public class GetAuthorByIdQueryHandler(
        ILogger<GetAuthorByIdQueryHandler> logger,
        IMapper mapper,
        IAuthorRepository authorRepo,
        IUserContext userContext) : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
    {
        public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetAuthorByIdQueryHandler invoked.");
            logger.LogInformation($"Request AuthorId: {request.AuthorID}");

            // Validate user permissions
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to get Author by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("You do not have permission to get Author details.");
            }

            // Fetch author from repository
            var author = await authorRepo.GetAuthorByIdAsync(request.AuthorID);
            if (author == null)
            {
                logger.LogWarning("Author not found. AuthorId: {AuthorId}", request.AuthorID);
                throw new ResourceNotFound(nameof(Author), request.AuthorID.ToString());
            }

            // Map to DTO
            var authorDto = mapper.Map<AuthorDto>(author);
            return authorDto;
        }
    }
}