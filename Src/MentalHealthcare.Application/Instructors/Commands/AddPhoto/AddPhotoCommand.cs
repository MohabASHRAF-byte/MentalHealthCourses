using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Instructors.Commands.AddPhoto
{
    public class AddPhotoCommand : IRequest<string>
    {

        public int InstructorsId { get; set; }
        public IFormFile File { get; set; } = default!;




    }
}
