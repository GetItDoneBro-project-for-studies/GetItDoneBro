using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project ID is required");

        RuleFor(x => x.Body.Name)
            .MaximumLength(100)
            .WithMessage("Project name must not exceed 100 characters");

        RuleFor(x => x.Body.Description)
            .MaximumLength(500)
            .WithMessage("Project description must not exceed 500 characters");
    }
}
