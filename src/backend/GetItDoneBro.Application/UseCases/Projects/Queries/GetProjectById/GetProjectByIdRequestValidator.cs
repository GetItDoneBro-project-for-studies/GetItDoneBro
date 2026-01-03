using FluentValidation;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

public class GetProjectByIdRequestValidator : AbstractValidator<GetProjectByIdRequest>
{
    public GetProjectByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project ID is required");
    }
}
