using FluentValidation;
using GetItDoneBro.Domain.Entities;

namespace GetItDoneBro.Application.Usecases.Projects.Commands;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.");

        RuleFor(x => x.Description)
            .MaximumLength(Project.MaxDescriptionLength)
            .WithMessage($"Description cannot exceed {Project.MaxDescriptionLength} characters.");
    }
}
