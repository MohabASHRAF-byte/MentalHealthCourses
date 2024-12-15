using AutoMapper;
using MediatR;
using MentalHealthcare.Application.ContactUs.Commands.Create;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Commands.Change_Read_state;

public class ChangeReadStateCommandHandler(
    ILogger<SubmitContactUsCommandHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper,
    IUserContext userContext
    ):IRequestHandler<ChangeReadStateCommand>
{
    public async Task Handle(ChangeReadStateCommand request, CancellationToken cancellationToken)
    {
        
        logger.LogInformation("Handling ChangeReadStateCommand");
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.IsAuthorized([UserRoles.Admin]))
        {
            logger.LogWarning("Unauthorized attempt to change contactUs Read state by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission to change contactUs Read state");
        }
        await dbRepository.ChangeReadState(request.FormId, request.State);
    }
}