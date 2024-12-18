using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Articles.Commands.AddArticle;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Create
{
    public class AddMeditationCommandHandler(
    IMeditationRepository meditationRepository,
    IConfiguration configuration,
    // IConfiguration configuration,
    //UserContext userContext,
    //  IAdminRepository adminRepository,
    IMapper mapper,
   ILogger<AddMeditationCommandHandler> logger,
    IMediator mediator) : IRequestHandler<AddMeditationCommand, AddMeditationCommandResponse>
    {
        public async Task<AddMeditationCommandResponse> Handle(AddMeditationCommand request, CancellationToken cancellationToken)
        {



            //TODO : Check if the current user is authorized
            //var currentUser = userContext.GetCurrentUser();
            //if (currentUser == null || !currentUser.HasRole(UserRoles.Admin))
            //    throw new UnauthorizedAccessException();
            //var admin = await adminRepository.GetAdminByIdentityAsync(currentUser.Id);

            //TODO: Create the Meditation entity from the command
            var meditation = new Meditation
            {
                Title = request.Title,
                Content = request.Content,
                UploadedById = request.UploadedById,
                CreatedDate = request.CreatedDate = DateTime.Now
            };



            //TODO : Validation: The code checks
            //if the Title and Content properties are not null
            //or whitespace, throwing an exception if they are invalid.
            var result = await mediator.Send(meditation, cancellationToken);
            if (string.IsNullOrEmpty(request.Content) ||
           string.IsNullOrEmpty(request.Title))
                logger.LogError("One or more Of Fields in Required Data  is Empty");




            //TODO: Save The article In DB using the repository
            var New_Meditation = await meditationRepository.AddMeditationAsync(meditation);
            logger.LogInformation("meditation created successfully with This Information: {ArticleId}", result);

            var response = new AddMeditationCommandResponse
            { MeditationID = New_Meditation };
            return response;













        }
    }
}