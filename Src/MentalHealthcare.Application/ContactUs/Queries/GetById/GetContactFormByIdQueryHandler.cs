using AutoMapper;
using MediatR;
using MentalHealthcare.Application.ContactUs.Commands.Create;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Queries.GetById;

public class GetContactFormByIdQueryHandler(
    ILogger<GetContactFormByIdQueryHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper
) : IRequestHandler<GetContactFormByIdQuery, ContactUsForm>
{
    public async Task<ContactUsForm> Handle(GetContactFormByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling GetContactFormByIdQueryHandler for Contact {request.Id}");
        var form = await dbRepository.GetFromById(request.Id);
        return form;
    }
}