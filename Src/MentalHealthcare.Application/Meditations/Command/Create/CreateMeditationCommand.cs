using MediatR;
using MentalHealthcare.Application.Instructors.Commands.Create;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Create
{
    public class CreateMeditationCommand : IRequest<CreateMeditationCommandResponse>
    {

        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        public IFormFile? Image { get; set; } = default!;





    }
}
