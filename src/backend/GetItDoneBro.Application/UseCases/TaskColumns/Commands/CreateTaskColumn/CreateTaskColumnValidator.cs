using FluentValidation;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.CreateTaskColumn;

public class CreateTaskColumnValidator : AbstractValidator<CreateTaskColumnCommand>
{
    public CreateTaskColumnValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Column name is required")
            .MaximumLength(100)
            .WithMessage("Column name must not exceed 100 characters");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Order index must be non-negative");
    }
}
