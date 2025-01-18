using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Queries.GetById
{
    public class GetInstructorByIdQuery : IRequest<InstructorDto>
    {

        public int instructorid { get; set; }


    }
}
