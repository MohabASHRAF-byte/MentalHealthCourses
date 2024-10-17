using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Application.Meditations.Commands.Add_Meditation;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Delete__Meditation
{
    public class Delete_Meditation_CommandHandler(
        ILogger<Delete_Meditation_CommandHandler> logger,
    IMeditation _meditation,
            IMapper mapper
        ) : IRequestHandler<Delete_Meditation_Command, OperationResult<string>>
 {
        public async Task<OperationResult<string>> Handle(Delete_Meditation_Command request, CancellationToken cancellationToken)
        {

            var MeditationO = await _meditation.GetById(request.AId);

            if (MeditationO is null)
            {
                return OperationResult<string>.Failure("This Meditation Not Found!", StateCode.Forbidden);
            }


            var Result = await _meditation.DeleteMeditationAsync(MeditationO);


            return OperationResult<string>.SuccessResult("The Meditation has been Deleted Successfully!.");



        }
    }
}
