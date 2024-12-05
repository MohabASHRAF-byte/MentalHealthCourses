using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommand : IRequest
    {

        public int ArticleId { get; set; }
        public string title { get; set; }



    }
}
