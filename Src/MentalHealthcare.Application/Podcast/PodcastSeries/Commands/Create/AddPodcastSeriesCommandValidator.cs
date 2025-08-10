using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Podcast.PodcastSeries.Commands.Create
{
    public class AddPodcastSeriesCommandValidator : AbstractValidator<AddPodcastSeriesCommand>
    {



        public AddPodcastSeriesCommandValidator()
        {
            RuleFor(x => x.NameOfTopic)
                .NotEmpty().WithMessage("Topic name is required.")
                .MaximumLength(255).WithMessage("Topic name cannot exceed 255 characters.");

            RuleFor(x => x.PodcasterName)
                .NotEmpty().WithMessage("Podcaster name is required.");
        }











    }
}
