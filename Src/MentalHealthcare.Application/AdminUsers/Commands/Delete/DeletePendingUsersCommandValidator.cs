using FluentValidation;
using MentalHealthcare.Application.validations;

namespace MentalHealthcare.Application.AdminUsers.Commands.Delete;

public class DeletePendingUsersCommandValidator : AbstractValidator<DeletePendingUsersCommand>
{
    public DeletePendingUsersCommandValidator()
    {
        RuleForEach(x => x.PendingUsers)
            .CustomIsValidEmail();
    }
}