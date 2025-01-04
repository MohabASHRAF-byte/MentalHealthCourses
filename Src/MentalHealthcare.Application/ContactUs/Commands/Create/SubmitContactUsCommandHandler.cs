using AutoMapper;
using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Commands.Create;

public class SubmitContactUsCommandHandler(
    ILogger<SubmitContactUsCommandHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper
) : IRequestHandler<SubmitContactUsCommand, int>
{
    public async Task<int> Handle(SubmitContactUsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting SubmitContactUsCommand for Name: {Name}, Email: {Email}", request.Name, request.Email);

        try
        {
            // Map the request to the entity
            logger.LogInformation("Mapping SubmitContactUsCommand to ContactUsForm entity.");
            var form = mapper.Map<ContactUsForm>(request);
            form.CreatedDate = DateTime.UtcNow;

            // Save the form to the database
            logger.LogInformation("Saving ContactUsForm for Name: {Name}, Email: {Email}", form.Name, form.Email);
            var newFormId = await dbRepository.CreateAsync(form);

            // Log the successful addition
            logger.LogInformation("ContactUsForm with ID: {FormId} was successfully added for Name: {Name}", newFormId, form.Name);

            // TODO: Send notification to the admin
            logger.LogInformation("Notification to admin for ContactUs submission is pending implementation.");

            return newFormId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while handling SubmitContactUsCommand for Name: {Name}, Email: {Email}", request.Name, request.Email);
            throw;
        }
    }
}