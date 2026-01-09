using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

public class DeleteProjectValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project ID is required");
    }
}