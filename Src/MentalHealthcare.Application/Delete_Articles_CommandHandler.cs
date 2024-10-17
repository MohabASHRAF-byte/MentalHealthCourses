using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Articls.Commands.Delete__Articles
{
    internal class Delete_Articles_CommandHandler(
        ILogger<Delete_Articles_CommandHandler> logger,
    IArticleRepository _articleRepository,
            IMapper mapper
        ) : IRequestHandler<Delete_Articles_Command, OperationResult<string>>
    {
        public async Task<OperationResult<string>> Handle(Delete_Articles_Command request, CancellationToken cancellationToken)
        {
            var existingArticle = await _articleRepository.GetById(request.AId);

            if (existingArticle is null)
            {
                return OperationResult<string>.Failure("This Article Not Found!", StateCode.Forbidden);
            }


            var Result = await _articleRepository.DeleteArticlAsync(existingArticle);


            return OperationResult<string>.SuccessResult("The Article has been Deleted Successfully!.");


        }
    }
}
