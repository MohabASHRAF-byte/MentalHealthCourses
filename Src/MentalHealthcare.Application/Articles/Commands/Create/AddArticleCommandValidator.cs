using FluentValidation;
using MentalHealthcare.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MentalHealthcare.Application.Articles.Commands.Create
{
    public class AddArticleCommandValidator : AbstractValidator<AddArticleCommand>
    {

        public AddArticleCommandValidator()
        {
            // Title validation
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .Length(1, Global.ArticleNameMaxLength)
                .WithMessage($"Title length must be between 1 and {Global.ArticleNameMaxLength} characters.");

            // Content validation
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .Length(1, Global.ArticleDescriptionMaxLength)
                .WithMessage($"Content length must be between 1 and {Global.ArticleDescriptionMaxLength} characters.");

            // AuthorId validation
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");

            // UploadedById validation
            RuleFor(x => x.UploadedById)
                .GreaterThan(0)
                 .WithMessage("UploadedById must be greater than 0.");


            RuleFor(x => x.Image_Article)
           .NotNull()
     .WithMessage("An image is required.")
     .Must(IsImage)
     .WithMessage("The uploaded file must be a valid image (e.g., JPG, PNG, GIF).");
            // CreatedDate validation
            RuleFor(x => x.CreatedDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("CreatedDate cannot be in the future.");

            // UploadedBy validation
            RuleFor(x => x.UploadedBy)
                .NotEmpty()
                .WithMessage("UploadedBy is required.");

            // Author validation
            RuleFor(x => x.Author)
                .NotEmpty()
                .WithMessage("Author is required.");
        }



        private bool IsImage(IFormFile file)
        {
            if (file == null) return false;

            // Check MIME types for common image formats
            var validMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

            // Check file extensions as a fallback
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

            return validMimeTypes.Contains(file.ContentType.ToLower()) || validExtensions.Contains(fileExtension);
        }



    }
}
