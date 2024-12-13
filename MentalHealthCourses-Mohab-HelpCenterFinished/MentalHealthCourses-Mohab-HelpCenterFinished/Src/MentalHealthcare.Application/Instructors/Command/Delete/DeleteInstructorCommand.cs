using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Command.Delete
{
    public class DeleteInstructorCommand : IRequest
    {
        public int InstructorId { get; set; }
    }
}
