using FluentValidation;

namespace MentalHealthcare.Application.Articles.Commands.AddArticle
{
    public class AddArticleCommandValidator : AbstractValidator<AddArticleCommand>
    {
        public AddArticleCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Image_Article)
                .NotEmpty().WithMessage("At least one image is required.")
                .ForEach(file => file.Must(f => f.FileName.EndsWith(".jpg") || f.FileName.EndsWith(".jpeg") || f.FileName.EndsWith(".png"))
                .WithMessage("Image must be in .jpg, .jpeg, or .png format."));

            RuleFor(x => x.CreatedDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future.");

            RuleFor(x => x.Author)
                .NotNull().WithMessage("Author information is required.");
        }
    }
}
