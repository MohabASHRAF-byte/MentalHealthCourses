using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articls.Queries.GetAll;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Queries.GetAllM
{
    public class GetAll_Meditation_QueryHandler(
    ILogger<GetAll_Meditation_QueryHandler> logger,
    IMeditation _meditation,
    IMapper mapper) : IRequestHandler<GetAll_Meditation_Query, PageResult<MeditationDto>>
    {
      

       async Task <PageResult<MeditationDto>> IRequestHandler<GetAll_Meditation_Query, PageResult<MeditationDto>>.Handle(GetAll_Meditation_Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving all Articles.");

            var AllArticles =
          await _meditation.GetAllAsync(request.SearchText, request.PageNumber, request.PageSize);
            var meditationDtos = mapper.Map<IEnumerable<MeditationDto>>(AllArticles.Item2);

            var count = AllArticles.Item1;
            var ret = new PageResult<MeditationDto>(meditationDtos, count, request.PageSize, request.PageNumber);
            return ret;





        }
    }
}
