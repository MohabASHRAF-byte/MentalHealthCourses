using MediatR;
using MentalHealthcare.Application.Common;

namespace MentalHealthcare.Application.Articls.Queries.GetById
{
    public class GetByTitleAsyncQuery : IRequest<OperationResult<ArticlesDto>>
    {


        public int ArticleId { get; set; }

        public GetByTitleAsyncQuery(int id)
        {
            id = ArticleId;
        }


    }
}
