using FluentValidation;
using MentalHealthcare.Application.Advertisement.Commands.Create;
using MentalHealthcare.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Commands.Ctreate
{
    public class createArticleValidations : AbstractValidator<CreateArticleCommand>
    {

        public createArticleValidations()
        {
            RuleFor(x => x.Title)
           .Length(1, Global.ArticleTitleMaxLength)
           .WithMessage($"Article Title length must be between 1 and {Global.ArticleTitleMaxLength}");
            RuleFor(x => x.Content)
                .Length(1, Global.ArticleContent)
                .WithMessage(
                    $"Article Content length must be between 1 and {Global.ArticleContent} characters.");
            RuleFor(x => x.Images)
                .NotEmpty()
                .WithMessage($"Article Must have at least one image.");
        }




    }
}
