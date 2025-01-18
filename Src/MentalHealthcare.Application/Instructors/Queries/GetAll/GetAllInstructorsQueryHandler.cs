using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Courses.Course.Queries.GetAll;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Domain.Repositories.Course;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Queries.GetAll
{
    public class GetAllInstructorsQueryHandler(
    ILogger<GetAllInstructorsQueryHandler> logger,
    IInstructorRepository insRepo,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetAllInstructorsQuery, PageResult<InstructorDto>>
    {
        public async Task<PageResult<InstructorDto>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
        {



            logger.LogInformation("Handling GetAllInstructorsQuery");
            var currentUser = userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            {
                logger.LogWarning("Unauthorized attempt to get all Instructors by user: {UserId}", currentUser?.Id);
                throw new ForBidenException("Don't have the permission to get all Instructors");
            }

            var Instructors = await insRepo.GetInstructorsAsync(request.SearchText,
            request.PageNumber, request.PageSize);
            var InstructorsDto = mapper.Map<IEnumerable<InstructorDto>>(Instructors.Item2);

            return new PageResult<InstructorDto>(InstructorsDto, Instructors.Item1, request.PageSize, request.PageNumber);







        }
    }
}
