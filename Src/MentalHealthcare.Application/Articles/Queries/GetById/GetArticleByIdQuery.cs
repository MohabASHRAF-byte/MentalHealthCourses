using MediatR;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Queries.GetById
{
    public class GetArticleByIdQuery : IRequest<ArticleDto>
    {

        public int ArticleID { get; set; }

    }
}
