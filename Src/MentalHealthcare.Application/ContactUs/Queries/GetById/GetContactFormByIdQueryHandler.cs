using AutoMapper;
using MediatR;
using MentalHealthcare.Application.ContactUs.Commands.Create;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Queries.GetById;

public class GetContactFormByIdQueryHandler(
    ILogger<GetContactFormByIdQueryHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<GetContactFormByIdQuery, ContactUsForm>
{
    public async Task<ContactUsForm> Handle(GetContactFormByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || !currentUser.IsAuthorized([UserRoles.Admin]))
        {
            logger.LogWarning("Unauthorized attempt to get contactUs by user: {UserId}", currentUser?.Id);
            throw new ForBidenException("Don't have the permission get contactUs");
        }
        logger.LogInformation($"Handling GetContactFormByIdQueryHandler for Contact {request.Id}");
        var form = await dbRepository.GetFromById(request.Id);
        return form;
    }
}