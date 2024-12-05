using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Commands.Create;

public class SubmitContactUsCommandHandler(
    ILogger<SubmitContactUsCommandHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper
    ):IRequestHandler<SubmitContactUsCommand, int>
{
    public async Task<int> Handle(SubmitContactUsCommand request, CancellationToken cancellationToken)
    {
       logger.LogInformation($"Submit ContactUs Command");
        //todo 
        //send notification to the admin
       var form = mapper.Map<ContactUsForm>(request);
       form.CreatedDate = DateTime.UtcNow;
       
        var newFormId = await dbRepository.CreateAsync(form);
        logger.LogInformation("SubmitContactUsCommand for @{n1} was added with name @{n2}",newFormId,form.Name);
        return newFormId;
    }
}