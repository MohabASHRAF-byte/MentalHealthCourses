using MediatR;

namespace MentalHealthcare.Application.TermsAndConditions.Commands.Delete;

public class DeleteTermCommand:IRequest
{
    public int TermId { get; set; }
}