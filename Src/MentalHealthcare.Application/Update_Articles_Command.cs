using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Articls.Commands.Update_Articles
{
    public class Update_Articles_Command : IRequest<OperationResult<string>>
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
