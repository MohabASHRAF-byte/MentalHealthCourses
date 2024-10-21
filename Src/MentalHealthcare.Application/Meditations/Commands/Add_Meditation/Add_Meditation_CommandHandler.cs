using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articls.Commands.Add_Articles;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Add_Meditation
{
    internal class Add_Meditation_CommandHandler(
        ILogger<Add_Meditation_CommandHandler> logger,
    IMeditation _meditation,
            IMapper mapper
        ) : IRequestHandler<Add_Meditation_Command, OperationResult<string>>
    {

        
        public async Task<OperationResult<string>> Handle(Add_Meditation_Command request, CancellationToken cancellationToken)
        {
            

            //To DO :  Check if
            //the Data of article (Content-Author.Name...ETC) didn't Written By Admin During Uploading 

            if (string.IsNullOrEmpty(request.Content) ||
            string.IsNullOrEmpty(request.Title) ||
           DateTime.Now == default(DateTime))
                
                logger.LogError("One or more Field in Required Data  is Empty");

            { return OperationResult<string>.Failure("Please insert The Required Information.", StateCode.Forbidden); }


            //To DO :  Check if the article already exists or no
            var exisitMeditation = await _meditation.GetById(request.MeditationId);
            if (exisitMeditation != null)
            {

                return OperationResult<string>.Failure("Already Exist", StateCode.Forbidden);

            }

            else
            {
                var MeditationMapper = mapper.Map<Meditation>(request);

                var Result = await _meditation.AddMeditationsync(MeditationMapper);


                return OperationResult<string>.SuccessResult(Result, "Success");
            }








        }
    }
}
        
    

