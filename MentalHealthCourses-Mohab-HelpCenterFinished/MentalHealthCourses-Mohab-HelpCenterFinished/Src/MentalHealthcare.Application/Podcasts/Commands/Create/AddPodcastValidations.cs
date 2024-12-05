using FluentValidation;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Podcasts.Commands.Create
{
    public class AddPodcastValidations : AbstractValidator<AddPodcastCommand>
    {
        public AddPodcastValidations()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(Global.MaxNameLength).WithMessage($"Name must not exceed {Global.MaxNameLength} characters.");

            RuleFor(p => p.About)
                .MaximumLength(1000).WithMessage("About must not exceed 1000 characters.");

            RuleFor(p => p.PodcastDescription)
                .NotEmpty().WithMessage("At least one podcast description file is required.")
                .ForEach(file => file.Must(f => f.FileName.EndsWith(".mp3") || f.FileName.EndsWith(".m4a") || f.FileName.EndsWith(".aac") || f.FileName.EndsWith(".wav") || f.FileName.EndsWith(".ogg"))
                .WithMessage("Podcast description must be in .mp3, .m4a, .aac, .wav, or .ogg format."));

            RuleFor(p => p.PodcastLength)
                .GreaterThan(0).WithMessage("Podcast length must be greater than 0.");

            RuleFor(p => p.PodCaster)
                .NotNull().WithMessage("PodCaster information is required.");

            RuleFor(p => p.CreatedDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedDate cannot be in the future.");
        }
    }
}
