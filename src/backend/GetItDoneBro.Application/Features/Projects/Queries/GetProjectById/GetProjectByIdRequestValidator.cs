using FluentValidation;

namespace GetItDoneBro.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdRequestValidator : AbstractValidator<GetProjectByIdRequest>
{
    public GetProjectByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project ID is required");
    }
}
