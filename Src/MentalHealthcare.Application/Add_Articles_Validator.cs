using FluentValidation;
using MentalHealthcare.Domain.Repositories;

namespace MentalHealthcare.Application.Articls.Commands.Add_Articles
{
    public class Add_Articles_Validator : AbstractValidator<Add_Articles_Command>
    {
        private readonly IArticleRepository _articleRepository;


        #region Ctor
        public Add_Articles_Validator(IArticleRepository articleRepository)
        {
            ValidationRules();
            _articleRepository = articleRepository;
        }
        #endregion


        #region Actions
        public void ValidationRules()
        {
            RuleFor(x => x.UploadedBy)
               .NotEmpty().WithMessage("Please enter your Name!")
               .NotNull();

            RuleFor(x => x.Author.Name)
                    .NotEmpty().WithMessage("Please insert The  Author Name.")
                    .MaximumLength(20)
                    .NotNull().WithMessage("The Author Name Has a Null Value!");

            RuleFor(x => x.PhotoUrl)
                        .NotEmpty().WithMessage("Please Attach The  Thumbnail.")
                        .NotNull();

            RuleFor(x => x.Content)
                       .NotEmpty().WithMessage("Please Attach The  Content of Article.")
                       .NotNull();

            RuleFor(x => x.CreatedDate)
                        .NotEmpty().WithMessage("Please Enter The  Date.")
                        .NotNull().WithMessage("The Creation Date is a Null Value!")
                        .Must(BeAValidDate).WithMessage("The Creation Date is not valid!");
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Please enter the Title of the Article.");

            RuleFor(x => x.ArticleId)
                .MustAsync(async (key, cancellation) => !await _articleRepository.IsExist(key))
                .WithMessage("Article already exists.");
            bool BeAValidDate(DateTime date)
            {
                return !date.Equals(default(DateTime));
            }


            RuleFor(A => A.ArticleId)
                       .MustAsync(async (Key, CancellationToken) => !await _articleRepository.IsExist(Key))
                       .WithMessage("Exist");







        }











        #endregion
    }
}
