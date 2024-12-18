using FluentValidation;
using Microsoft.AspNetCore.Http;
using MentalHealthcare.Application.Podcasts.Commands.Create;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Podcasts.Commands.Create
{
    public class AddPodcastCommandValidator : AbstractValidator<AddPodcastCommand>
    {
        public AddPodcastCommandValidator()
        {
            // Title must not be empty
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Podcast title is required.")
                .MaximumLength(255).WithMessage("Podcast title cannot exceed 255 characters.");

            // Description must not be empty
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Podcast description is required.")
                .MaximumLength(1000).WithMessage("Podcast description cannot exceed 1000 characters.");

            // File URL must not be null or empty
            RuleFor(x => x.Url)
                .NotNull().WithMessage("Podcast audio file is required.")
                .Must(BeAValidFile).WithMessage("Invalid audio file format. Only MP3 and M4A are allowed.");

            // Podcast length should be greater than 0 and not exceed the max allowed size (e.g., 100MB)
            RuleFor(x => x.PodcastLength)
                .GreaterThan(0).WithMessage("Podcast length must be greater than 0 seconds.");
        }

        // Custom file validation to ensure it's MP3 or M4A
        private bool BeAValidFile(IFormFile file)
        {
            if (file == null)
                return false;

            var allowedExtensions = new[] { ".mp3", ".m4a" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName)?.ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
