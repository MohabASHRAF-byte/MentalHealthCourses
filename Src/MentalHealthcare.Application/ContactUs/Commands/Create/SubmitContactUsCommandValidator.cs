using FluentValidation;
using MentalHealthcare.Application.TermsAndConditions.Commands.Create;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.ContactUs.Commands.Create;

public class SubmitContactUsCommandValidator:AbstractValidator<SubmitContactUsCommand>
{
    public SubmitContactUsCommandValidator()
    {
       RuleFor(f=>f.Name)
           .Length(1,Global.ContactUsMaxNameLength)
           .WithMessage($"Name must be between 1 and {Global.ContactUsMaxNameLength} characters long.");
       
       RuleFor(f=>f.Message)
           .Length(1,Global.ContactUsMaxMsgLength)
           .WithMessage($"Message must be between 1 and {Global.ContactUsMaxMsgLength} characters long.");
       RuleFor(f=>f.Email)
           .NotNull()
           .When(f => f.Email != null) 
           .EmailAddress()
           .Length(1,Global.ContactUsMaxEmailLength)
           .WithMessage("Invalid email address.");
       RuleFor(f => f.PhoneNumber)
           .Cascade(CascadeMode.Stop)
           .Matches(@"^\+?[0-9\s\-\(\)]+$")
           .When(f => !string.IsNullOrWhiteSpace(f.PhoneNumber))
           .WithMessage("Invalid phone number format.");


    }
}

