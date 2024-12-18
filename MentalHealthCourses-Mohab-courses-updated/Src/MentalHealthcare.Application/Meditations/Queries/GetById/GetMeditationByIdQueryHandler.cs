using AutoMapper;
using MediatR;
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
    public class GetMeditationByIdQueryHandler(
    ILogger<GetMeditationByIdQueryHandler> logger,
    IMapper mapper,
    IMeditationRepository meditationRepository
) : IRequestHandler<GetMeditationByIdQuery, MeditationDto>
    {
        public async Task<MeditationDto> Handle(GetMeditationByIdQuery request, CancellationToken cancellationToken)
        {



            var Selected_Meditation = await meditationRepository.GetMeditationsById(request.Id);

            var meditationDto = mapper.Map<MeditationDto>(Selected_Meditation); return meditationDto;






        }
    }
}