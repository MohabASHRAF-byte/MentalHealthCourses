using AutoMapper;
using MediatR;
using MentalHealthcare.Application.ContactUs.Commands.Change_Read_state;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Commands.Change_Read_State;

public class ChangeReadStateCommandHandler(
    ILogger<ChangeReadStateCommandHandler> logger,
    IContactUsRepository dbRepository,
    IUserContext userContext
) : IRequestHandler<ChangeReadStateCommand>
{
    public async Task Handle(ChangeReadStateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting ChangeReadStateCommand for Form ID: {FormId} with State: {State}",
            request.FormId, request.State);

        // Validate authorization
        var currentUser = userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);
        logger.LogInformation("User {UserId} authorized to change Read state.", currentUser.Id);

        // Update the read state in the database
        logger.LogInformation("Changing Read state for Form ID: {FormId} to {State}", request.FormId, request.State);
        await dbRepository.ChangeReadState(request.FormId, request.State);

        logger.LogInformation("Successfully changed Read state for Form ID: {FormId} to {State}", request.FormId,
            request.State);
    }
}