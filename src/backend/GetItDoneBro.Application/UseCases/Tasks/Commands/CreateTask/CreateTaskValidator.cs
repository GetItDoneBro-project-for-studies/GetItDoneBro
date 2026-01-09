using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.CreateTask;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");

        RuleFor(x => x.TaskColumnId)
            .NotEmpty()
            .WithMessage("Task column ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Task title is required")
            .MaximumLength(200)
            .WithMessage("Task title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Task description is required")
            .MaximumLength(2000)
            .WithMessage("Task description must not exceed 2000 characters");

        RuleFor(x => x.ImageUrl)
            .Must(uri => uri == null || uri.IsAbsoluteUri)
            .WithMessage("Image URL must be a valid absolute URI")
            .When(x => x.ImageUrl != null);
    }
}
