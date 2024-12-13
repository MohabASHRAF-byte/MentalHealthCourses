using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Authors.Commands.Delete
{
    public class DeleteAuthorCommand : IRequest
    {
        public int AuthorId { get; set; }
    }
}
