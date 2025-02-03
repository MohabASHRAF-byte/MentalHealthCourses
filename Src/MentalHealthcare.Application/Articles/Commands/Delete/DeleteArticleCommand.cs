using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Delete
{
    public class DeleteArticleCommand : IRequest
    {
        public int Id { get; set; }

    }
}