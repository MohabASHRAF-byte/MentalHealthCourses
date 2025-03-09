using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Instructors.Queries.GetAll;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetTrainers
{
    public class GetTrainersQueryHandler(
        ILogger<GetTrainersQueryHandler> logger,
        IInstructorRepository insRepo,
        IMapper mapper,
        IUserContext userContext
    ) : IRequestHandler<GetTrainersQuery, PageResult<InstructorDto>>
    {
        public async Task<PageResult<InstructorDto>> Handle(GetTrainersQuery request, CancellationToken cancellationToken)
        {



            logger.LogInformation("Handling GetAllInstructorsQuery");
            userContext.EnsureAuthorizedUser([UserRoles.User], logger);
            var Instructors = await insRepo.GetInstructorsAsync(request.SearchText,
                request.PageNumber, request.PageSize);
            var InstructorsDto = mapper.Map<IEnumerable<InstructorDto>>(Instructors.Item2);

            return new PageResult<InstructorDto>(InstructorsDto, Instructors.Item1, request.PageSize,
                request.PageNumber);








        }
    }
}
