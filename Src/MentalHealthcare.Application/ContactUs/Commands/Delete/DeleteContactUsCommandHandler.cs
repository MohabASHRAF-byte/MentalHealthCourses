using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Commands.Delete;

public class DeleteContactUsCommandHandler(
    ILogger<DeleteContactUsCommandHandler> logger,
    IContactUsRepository dbRepository,
    IUserContext userContext
) : IRequestHandler<DeleteContactUsCommand>
{
    public async Task Handle(DeleteContactUsCommand request, CancellationToken cancellationToken)
    {
        userContext.EnsureAuthorizedUser([UserRoles.Admin], logger);

        if (request.FormsId.Count < 1)
        {
            logger.LogInformation("DeleteContactUsCommandHandler.Handle: No forms selected");
            return;
        }

        logger.LogInformation(@"deleting {num} Forms ", request.FormsId.Count);
        await dbRepository.DeleteAsync(request.FormsId);
        logger.LogInformation(@"{num} form  deleted", request.FormsId.Count);
    }
}