using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Articls.Commands.Add_Articles
{
    public class Add_Articles_Command : IRequest<OperationResult<string>>
    {
        public int ArticleId { get; set; }
        public int AuthorId { get; set; }
        public int UploadedById { get; set; } // Foreign Key property
        public string Content { get; set; } = default!;
        public string PhotoUrl { get; set; } = default!;
        public string Title { get; set; } = default!;
        public DateTime CreatedDate { get; set; }

        public Admin UploadedBy { get; set; } = default!;
        public Author Author { get; set; } = default!;


    }
}
/* 
    
    public Admin UploadedBy { get; set; } = default!;
    
  */