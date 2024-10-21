using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Articls.Commands.Update_Articles
{
    public class Update_Articles_CommandHandler
   (IArticleRepository _articleRepository,
   ILogger<Update_Articles_CommandHandler> _logger,
            IMapper _mapper) : IRequestHandler<Update_Articles_Command, OperationResult<string>>
    {

        public async Task<OperationResult<string>>
          Handle(Update_Articles_Command request, CancellationToken cancellationToken)
        {
            // To Do : Check if Id Of Article Exist oR not
            var UpdatedArticle = await _articleRepository.GetById(request.ArticleId);

            if (UpdatedArticle is null)
            {
                //To Do : Not Exist :we Can't make updating on not exist data
                _logger.LogError("Article {} not found", request.ArticleId);
                return OperationResult<string>.Failure("Article NOT found");
            }
            //Exist  : ok , we can Update 
            else
            {
                var ArticleMapper = _mapper.Map<Article>(request);
                var Result = await _articleRepository.UpdateArticlAsync(ArticleMapper);
                return OperationResult<string>.SuccessResult("The Article has been Updated Successfully!.");
            }



        }
    }
}
