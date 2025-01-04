using MediatR;
using MentalHealthcare.Application.SystemUsers;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Queries.GetById;

public class GetContactFormByIdQueryHandler(
    ILogger<GetContactFormByIdQueryHandler> logger,
    IContactUsRepository dbRepository,
    IUserContext userContext
) : IRequestHandler<GetContactFormByIdQuery, ContactUsForm>
{
    public async Task<ContactUsForm> Handle(GetContactFormByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GetContactFormByIdQueryHandler for Contact Form ID: {Id}", request.Id);

        // Ensure the user is authorized
        var currentUser = userContext.EnsureAuthorizedUser( [UserRoles.Admin],logger);
        logger.LogInformation("User {UserId} authorized to access Contact Form ID: {Id}", currentUser.Id, request.Id);

        // Retrieve the contact form from the database
        logger.LogInformation("Fetching Contact Form ID: {Id} from the database", request.Id);
        var form = await dbRepository.GetFromById(request.Id);

        if (form == null)
        {
            logger.LogWarning("Contact Form ID: {Id} not found in the database", request.Id);
            throw new KeyNotFoundException($"Contact Form with ID {request.Id} not found.");
        }

        logger.LogInformation("Successfully fetched Contact Form ID: {Id} from the database", request.Id);

        return form;
    }
}