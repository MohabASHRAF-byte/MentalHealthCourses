using FluentValidation;
using MentalHealthcare.Domain.Repositories;

namespace MentalHealthcare.Application.Articls.Commands.Update_Articles
{
    public class Update_Articles_Validator : AbstractValidator<Update_Articles_Command>
    {
        private readonly IArticleRepository _articleRepository;

        public Update_Articles_Validator(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public void ValidationRules()
        {

            RuleFor(x => x.Author.Name)
                    .NotEmpty().WithMessage("Please insert The  Author Name.")
                    .MaximumLength(104)
                    .NotNull().WithMessage("The Author Name Has a Null Value!");

            RuleFor(x => x.PhotoUrl)
                        .NotEmpty().WithMessage("Please Attach The  Thumbnail.")
                        .NotNull();



            RuleFor(x => x.CreatedDate)
                        .NotEmpty().WithMessage("Please Enter The  Date.")
                        .NotNull().WithMessage("The Creation Date is a Null Value!"); ;

            RuleFor(x => x.UploadedBy)
                      .NotEmpty().WithMessage("Please Enter Your Name.")
                      .NotNull();


            RuleFor(A => A.Title)
   .MustAsync(async (model, Key, CancellationToken)
    => !await _articleRepository.IsExistDuringUpdate(Key, model.ArticleId))
                        .WithMessage("This Title Already Exist in Another Article");

            RuleFor(A => A.Content)
  .MustAsync(async (model, Key, CancellationToken)
   => !await _articleRepository.IsExistDuringUpdate(Key, model.ArticleId))
                       .WithMessage("This Title Already Exist in Another Article");

        }






    }
}
