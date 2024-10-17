using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Queries.GetByIdM
{
    public class GetByID_Query : IRequest<OperationResult<MeditationDto>>
    {


        public int MedtationId { get; set; }

        public GetByID_Query(int id)
        {
            id = MedtationId;
        }




    }
}
