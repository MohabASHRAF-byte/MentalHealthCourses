using MediatR;
using MentalHealthcare.Application.TermsAndConditions.Commands.Create;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.TermsAndConditions.Commands.Delete;

public class DeleteTermCommandHandler(
    ILogger<DeleteTermCommandHandler> logger,
    ITermsRepository termsRepository
) : IRequestHandler<DeleteTermCommand>
{
    public async Task Handle(DeleteTermCommand request, CancellationToken cancellationToken)
    {
        //todo
        //add auth
        logger.LogInformation(@"Delete Term with id {}", request.TermId);
        await termsRepository.DeleteAsync(request.TermId);
        logger.LogInformation(@"Term with id {} deleted", request.TermId);
    }
}