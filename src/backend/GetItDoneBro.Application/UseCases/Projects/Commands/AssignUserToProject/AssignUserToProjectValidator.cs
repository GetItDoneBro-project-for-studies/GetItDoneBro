using FluentValidation;
using GetItDoneBro.Application.UseCases.Projects.Commands.AssignUserToProject;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Commands.AssignUserToProject;

public class AssignUserToProjectValidator : AbstractValidator<AssignUserToProjectCommand>
{
    public AssignUserToProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Identyfikator projektu jest wymagany");

        RuleFor(x => x.KeycloakId)
            .NotEmpty()
            .WithMessage("Identyfikator uzytkownika Keycloak jest wymagany");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Nieprawidlowa rola uzytkownika");
    }
}