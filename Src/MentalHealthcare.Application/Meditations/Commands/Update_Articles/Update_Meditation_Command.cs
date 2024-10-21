using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Commands.Update_Articles
{
    public class Update_Meditation_Command : IRequest<OperationResult<string>>
    {
        public int ArticleId { get; set; }

        public int AuthorId { get; set; }
        public int UploadedById { get; set; }

        public Author Author { get; set; } = default!;

        public string Content { get; set; } = default!;

        public string PhotoUrl { get; set; } = default!;


        public Admin UploadedBy { get; set; } = default!;

        public DateTime CreatedDate { get; set; }
        public string Title { get; set; } = default!;












    }
}
