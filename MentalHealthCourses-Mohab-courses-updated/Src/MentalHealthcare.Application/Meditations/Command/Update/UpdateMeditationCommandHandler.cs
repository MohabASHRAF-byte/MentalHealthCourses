using AutoMapper;
using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Update
{
    public class UpdateMeditationCommandHandler(
   IMeditationRepository meditationRepository,
   IConfiguration configuration,
    IMapper mapper,
    //UserContext userContext,
    // IAdminRepository adminRepository,
    ILogger<UpdateMeditationCommandHandler> logger
   ) : IRequestHandler<UpdateMeditationCommand, int>
    {
        public async Task<int> Handle(UpdateMeditationCommand request, CancellationToken cancellationToken)
        {


            var Edited_Meditation = await meditationRepository.GetMeditationsById(request.MeditationId);
            if (Edited_Meditation == null)
            {
                logger.LogWarning("Meditation with ID {MeditationId} not found.", request.MeditationId);
            }


            logger.LogInformation(@"Updated term {}", request.Title);
            await meditationRepository.UpdateMeditationAsync(Edited_Meditation);

            return request.MeditationId;


        }
    }
}
