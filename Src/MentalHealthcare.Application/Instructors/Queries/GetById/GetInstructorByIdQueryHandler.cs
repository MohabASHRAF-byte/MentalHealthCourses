using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Instructors.Commands.Create;
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
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Queries.GetById
{
    public class GetInstructorByIdQueryHandler(
        ILogger<GetInstructorByIdQueryHandler> logger,
        IMapper mapper,
        IInstructorRepository insRepo,
        IConfiguration configuration,
        IUserContext userContext) : IRequestHandler<GetInstructorByIdQuery, InstructorDto>
    {
        public async Task<InstructorDto> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetInstructorByIdQueryHandler invoked.");
            logger.LogInformation($"GetInstructorByIdQueryHandler. Request: {request.instructorid}");
            userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
            var ins = await insRepo.GetInstructorByIdAsync(request.instructorid);
            var insDto = mapper.Map<InstructorDto>(ins);
            return insDto;
        }
    }
}