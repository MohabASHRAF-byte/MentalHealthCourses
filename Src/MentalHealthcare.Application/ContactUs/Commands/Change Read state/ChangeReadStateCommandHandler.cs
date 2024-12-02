using AutoMapper;
using MediatR;
using MentalHealthcare.Application.ContactUs.Commands.Create;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.ContactUs.Commands.Change_Read_state;

public class ChangeReadStateCommandHandler(
    ILogger<SubmitContactUsCommandHandler> logger,
    IContactUsRepository dbRepository,
    IMapper mapper
    ):IRequestHandler<ChangeReadStateCommand>
{
    public async Task Handle(ChangeReadStateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ChangeReadStateCommand");
        await dbRepository.ChangeReadState(request.FormId, request.State);
    }
}