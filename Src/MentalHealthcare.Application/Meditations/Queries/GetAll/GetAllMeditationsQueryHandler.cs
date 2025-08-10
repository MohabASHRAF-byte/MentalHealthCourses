using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Instructors.Queries.GetAll;
using MentalHealthcare.Application.Meditations.Command.Create;
using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Queries.GetAll
{
    public class GetAllMeditationsQueryHandler(
        
ILogger<CreateMeditationCommandHandler> logger,
IMapper mapper,
IMeditationRepository MedRepo,
IConfiguration configuration,
IUserContext userContext,
ILocalizationService localizationService
) : IRequestHandler<GetAllMeditationsQuery, PageResult<MeditationDto>>
    {
        public async Task<PageResult<MeditationDto>> Handle(GetAllMeditationsQuery request, CancellationToken cancellationToken)
        {


            logger.LogInformation(
               "Starting Handle method for GetAllMeditationsQuery. SearchText: {SearchText}, PageNumber: {PageNumber}, PageSize: {PageSize}",
               request.SearchText, request.PageNumber, request.PageSize);

            var currentUser = userContext.UserHaveAny([UserRoles.Admin, UserRoles.User], logger);
            int? userId = currentUser.HasRole(UserRoles.Admin) ? null : currentUser.SysUserId!.Value;

            logger.LogDebug("Calling repository to get all meditations for UserId: {UserId}", userId);
            var Meditations = await MedRepo.GetAllMeditationsAsync(request.SearchText,
                request.PageNumber, request.PageSize);
            var MeditationsDto = mapper.Map<IEnumerable<MeditationDto>>(Meditations.Item2);

            return new PageResult<MeditationDto>(MeditationsDto, Meditations.Item1, request.PageSize,
                request.PageNumber);







        }
    }
}
