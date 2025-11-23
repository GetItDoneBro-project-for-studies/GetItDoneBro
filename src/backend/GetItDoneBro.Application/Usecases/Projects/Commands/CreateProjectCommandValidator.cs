using FluentValidation;

namespace GetItDoneBro.Application.Usecases.Projects.Commands;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nazwa projektu jest wymagana.");

        RuleFor(x => x.Description)
            .MaximumLength(300)
            .WithMessage("Opis nie może przekraczać 300 znaków.");
    }
}
