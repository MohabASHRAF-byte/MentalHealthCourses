using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Add_Meditation
{
    public class Add_Meditation_Command : IRequest<OperationResult<string>>
    {

        public int MeditationId { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int UploadedById { get; set; } 
        public DateTime CreatedDate { get; set; }
        public Admin UploadedBy { get; set; } = default!;


    }
}
