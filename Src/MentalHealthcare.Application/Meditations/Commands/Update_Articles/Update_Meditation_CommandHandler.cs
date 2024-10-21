using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articls.Commands.Update_Articles;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Update_Articles
{
    public class Update_Meditation_CommandHandler(IMeditation _meditation,
   ILogger<Update_Meditation_CommandHandler> _logger,
            IMapper _mapper) : IRequestHandler<Update_Meditation_Command, OperationResult<string>>
    {
        public async Task<OperationResult<string>> Handle(Update_Meditation_Command request, CancellationToken cancellationToken)
        {
            // To Do : Check if Id Of Article Exist oR not
            var UpdateMeditation = await _meditation.GetById(request.ArticleId);
            if (UpdateMeditation is null)
            {
                //To Do : Not Exist :we Can't make updating on not exist data
                _logger.LogError("Meditation {} not found", request.ArticleId);
                return OperationResult<string>.Failure("Article NOT found");
            }
            //To Do :Exist  : ok , we can Update 
            else
            {
                var MeditationMapper = _mapper.Map<Meditation>(request);
                var Result = await _meditation.UpdateMeditationAsync(MeditationMapper);
                return OperationResult<string>.SuccessResult("The Article has been Updated Successfully!.");
            }




        }
    }
}
