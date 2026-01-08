using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

public class GetProjectByIdRequestValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project ID is required");
    }
}
