using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Delete__Meditation
{
    public class Delete_Meditation_Command : IRequest<OperationResult<string>>
    {

        public Article articleDeleted { get; set; }
        public int AId { get; set; }
    }
}
