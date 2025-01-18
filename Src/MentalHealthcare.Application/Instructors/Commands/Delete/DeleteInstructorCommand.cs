using AngleSharp.Io;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Commands.Delete
{
    public class DeleteInstructorCommand : IRequest
    {


        public int InstructorID { get; set; }

    }
}
