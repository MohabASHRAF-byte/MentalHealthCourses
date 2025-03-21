using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Meditations.Queries.GetAll;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Queries.GetById
{
    public class GetMeditationByIdQueryHandler(
 ILogger<CreateMeditationCommandHandler> logger,
IMapper mapper,
IMeditationRepository MedRepo,
IConfiguration configuration,
IUserContext userContext,
ILocalizationService localizationService
) : IRequestHandler<GetMeditationByIdQuery, MeditationDto>
    {

       async Task<MeditationDto> IRequestHandler<GetMeditationByIdQuery, MeditationDto>.Handle(GetMeditationByIdQuery request, CancellationToken cancellationToken)
        {



            logger.LogInformation("Handling GetMeditationByIdQuery for Meditation ID: {MeditationId}", request.Meditationid);

            // Authorize user
            logger.LogInformation("Authorizing user for retrieving meditation by ID.");
            var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            logger.LogInformation("User {UserId} authorized to retrieve meditation.", currentUser.Id);

            // Fetch meditation
            logger.LogInformation("Fetching meditation with ID: {MeditationId}", request.Meditationid);
            var meditation = await MedRepo.GetMeditationByIdAsync(request.Meditationid);
            if (meditation == null)
            {
                logger.LogWarning("Meditation with ID: {MeditationId} not found.", request.Meditationid);
                throw new ResourceNotFound(
                    "Meditation",
                    "تأمل",
                    request.Meditationid.ToString()
                );
            }

            // Map to DTO
            logger.LogInformation("Mapping meditation to DTO for Meditation ID: {MeditationId}.", request.Meditationid);
            var meditationDto = mapper.Map<MeditationDto>(meditation);

            logger.LogInformation("Successfully retrieved and mapped meditation with ID: {MeditationId}.", request.Meditationid);
            return meditationDto;




        }
    }

  
}
