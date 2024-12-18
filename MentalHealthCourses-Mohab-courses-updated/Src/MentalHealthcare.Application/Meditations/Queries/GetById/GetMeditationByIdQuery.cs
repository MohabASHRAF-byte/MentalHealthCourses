using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Queries.GetById
{
    public class GetMeditationByIdQuery : IRequest<MeditationDto>
    {




        public int Id { get; set; }








    }
}