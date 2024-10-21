using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Articls.Commands.Delete__Articles
{
    public class Delete_Articles_Command : IRequest<OperationResult<string>>
    {
        public Article articleDeleted { get; set; }
        public int AId { get; set; }

    }
}
