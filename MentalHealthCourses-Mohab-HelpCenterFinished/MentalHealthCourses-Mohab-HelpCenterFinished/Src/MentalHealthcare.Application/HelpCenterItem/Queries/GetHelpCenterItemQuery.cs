using MediatR;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.HelpCenterItem.Queries;

public class GetHelpCenterItemQuery:IRequest<List<Domain.Entities.HelpCenterItem>>
{
    public Global.HelpCenterItems itemType { get; set; }
}