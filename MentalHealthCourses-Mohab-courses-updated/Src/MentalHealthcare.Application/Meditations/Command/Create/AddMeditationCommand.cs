using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Create
{
    public class AddMeditationCommand : IRequest<AddMeditationCommandResponse>
    {
        [Required] public string Title { get; set; } = default!;
        [Required] public int UploadedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [Required] public string Content { get; set; } = default!;















    }
}
