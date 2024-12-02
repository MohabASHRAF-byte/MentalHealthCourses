// using MediatR;
// using MentalHealthcare.Domain.Repositories;
// using Microsoft.Extensions.Logging;
//
// namespace MentalHealthcare.Application.TermsAndConditions.Commands.Update;
//
// public class UpdateTermCommandHandler(
//     ILogger<UpdateTermCommandHandler> logger,
//     ITermsRepository termsRepository
//     ):IRequestHandler<UpdateTermCommand>
// {
//     public async Task Handle(UpdateTermCommand request, CancellationToken cancellationToken)
//     {
//         logger.LogInformation(@"Updated term {}",request.term.Name);
//         await termsRepository.Update(request.term);
//         
//     }
// }