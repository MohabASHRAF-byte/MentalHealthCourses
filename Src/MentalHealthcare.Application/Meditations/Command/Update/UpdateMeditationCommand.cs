using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Update
{
    public class UpdateMeditationCommand : IRequest<int>
    {

        public int Meditationid { get; set; }

        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        public IFormFile? Image { get; set; } = default!;









    }
}
