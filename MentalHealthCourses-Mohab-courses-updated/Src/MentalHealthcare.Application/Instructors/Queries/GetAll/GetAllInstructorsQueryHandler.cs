using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Meditations.Queries.GetAll;
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

namespace MentalHealthcare.Application.Instructors.Queries.GetAll
{
    public class GetAllInstructorsQueryHandler(
    ILogger<GetAllInstructorsQueryHandler> logger,
    IMapper mapper,
    IInstructorRepository InsRepo
    ) : IRequestHandler<GetAllInstructorsQuery, PageResult<InstructorDto>>
    {
        public async Task<PageResult<InstructorDto>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
        {

            //TODO : Add Autho 
            
            var ins = await InsRepo.GetAllInstructors(request.SearchText,
                request.PageNumber, request.PageSize, request.sortBy
                );
            var InsDto = mapper.Map<IEnumerable<InstructorDto>>(ins.Item2);

            return new PageResult<InstructorDto>(InsDto, ins.Item1, request.PageSize, request.PageNumber , request.SearchText);








        }
    }
}
