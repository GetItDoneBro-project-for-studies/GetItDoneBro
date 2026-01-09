using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.MoveTaskToColumn;

public class MoveTaskToColumnValidator : AbstractValidator<MoveTaskToColumnCommand>
{
    public MoveTaskToColumnValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task ID is required");

        RuleFor(x => x.TaskColumnId)
            .NotEmpty()
            .WithMessage("Task column ID is required");
    }
}
