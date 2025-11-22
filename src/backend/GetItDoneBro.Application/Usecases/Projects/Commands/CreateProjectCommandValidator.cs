using FluentValidation;

namespace GetItDoneBro.Application.Usecases.Projects.Commands;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.");

        RuleFor(x => x.Description)
            .MaximumLength(300)
            .WithMessage("Description cannot exceed 300 characters.");
    }
}
