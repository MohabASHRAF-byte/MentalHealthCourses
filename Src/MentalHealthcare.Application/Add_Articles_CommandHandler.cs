using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
namespace MentalHealthcare.Application.Articls.Commands.Add_Articles
{



    public class Add_Articles_CommandHandler
        (
        ILogger<Add_Articles_CommandHandler> logger,
    IArticleRepository _articleRepository,
            IMapper mapper
        ) : IRequestHandler<Add_Articles_Command, OperationResult<string>>
    {
        Admin? admin;

        public async Task<OperationResult<string>> Handle(Add_Articles_Command request, CancellationToken cancellationToken)
        {


            //To DO :  Check if
            //the Data of article (Content-Author.Name...ETC) didn't Written By Admin During Uploading 

            if (string.IsNullOrEmpty(request.Content) ||
            string.IsNullOrEmpty(request.Author.Name) ||
            string.IsNullOrEmpty(request.PhotoUrl))
                logger.LogError("One or more Of Fields in Required Data  is Empty");

            { return OperationResult<string>.Failure("Please insert The Required Information.", StateCode.Forbidden); }


            //To DO :  Check if the article already exists or no
            var existingArticle = await _articleRepository.GetById(request.ArticleId);
            if (existingArticle != null)
            {

                return OperationResult<string>.Failure("Already Exist", StateCode.Forbidden);

            }

            else
            {
                var ArticleMapper = mapper.Map<Article>(request);

                var Result = await _articleRepository.AddArticlAsync(ArticleMapper);


                return OperationResult<string>.SuccessResult(Result, "Success");
            }







        }
    }



}






/* public async Task<bool> Handle(Add_Articles_Command request, CancellationToken cancellationToken)
        {
            //To DO : Make Articles Must Have Content , Name of Author
            //and PhotoUrl

            if (string.IsNullOrEmpty(request.Content) ||
               string.IsNullOrEmpty(request.Author.Name)
               || string.IsNullOrEmpty(request.PhotoUrl))

            //To DO : if Admin Doesn't Enter Content , Name ,Author or PhotoUrl
            {
                logger.LogError("Please Enter Requried Data");
                return false;
            }

            else { logger.LogInformation("Created Succces "); }
            return true;








public Task<OperationResult<string>> Handle(Add_Articles_Command request, CancellationToken cancellationToken)
        {

            Admin? admin;

            if (string.IsNullOrEmpty(request.Content) ||
               string.IsNullOrEmpty(request.Author.Name)
               || string.IsNullOrEmpty(request.PhotoUrl))
            {
                logger.LogInformation("Invalid Article ");
                return OperationResult<string>.Failure("Bad Request", StateCode.BadRequest);
            }





        }











        }*/