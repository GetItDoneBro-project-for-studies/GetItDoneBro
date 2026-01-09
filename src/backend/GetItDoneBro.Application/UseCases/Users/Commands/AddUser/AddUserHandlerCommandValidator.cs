using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Users.Commands.AddUser;

public sealed class AddUserHandlerCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserHandlerCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}