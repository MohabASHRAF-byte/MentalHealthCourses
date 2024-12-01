using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Courses.Commands.Create;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using MentalHealthcare.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace MentalHealthcare.Application.TermsAndConditions.Commands.Create;

public class CreateTermCommandHandler(
    ILogger<CreateTermCommandHandler> logger,
    ITermsRepository termsRepository
    ):IRequestHandler<CreateTermCommand, int>
{
    public async Task<int> Handle(CreateTermCommand request, CancellationToken cancellationToken)
    {
        //todo 
        // add auth
        logger.LogInformation("CreateTermCommandHandler for @{}",request.Name);

        var term = new Domain.Entities.TermsAndConditions
        {
            Name = request.Name,
            Description = request.Description,
        };
        var newTermId = await termsRepository.AddAsync(term);
        logger.LogInformation("CreateTermCommandHandler for @{} was added with name @{}",newTermId,term.Name);
        return newTermId;
    }
}