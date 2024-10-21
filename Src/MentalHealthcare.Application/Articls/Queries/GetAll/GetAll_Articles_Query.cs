using MediatR;
using MentalHealthcare.Application.Common;
using System.ComponentModel.DataAnnotations;

namespace MentalHealthcare.Application.Articls.Queries.GetAll
{
    public class GetAll_Articles_Query : IRequest<PageResult<ArticlesDto>>
    {
        [MaxLength(100)]
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;


    }
}
