using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Meditations.Queries.GetAllM;
using MentalHealthcare.Application.Meditations.Queries.GetByIdM;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Queries.GetById
{
    internal class GetByID_QueryHandler(
    ILogger<GetByID_Query> logger,
    IMeditation _meditation,
    IMapper mapper) : IRequestHandler<GetByID_Query, OperationResult<MeditationDto>>

    {
        public async Task<OperationResult<MeditationDto>> Handle(GetByID_Query request, CancellationToken cancellationToken)
        {


            var MeditationDto = await _meditation.GetById(request.MedtationId);
            if (MeditationDto == null)
            {
                throw new DllNotFoundException("Meditation Not Found.");
                // Return a failure result indicating that the article was not found
                return  OperationResult<MeditationDto>.Failure("meditation not found.", StateCode.NotFound);
            }

            var meditation = mapper.Map<MeditationDto>(MeditationDto);


            return OperationResult<MeditationDto>.SuccessResult(meditation , "meditation retrieved successfully.");



        }
    }
}
