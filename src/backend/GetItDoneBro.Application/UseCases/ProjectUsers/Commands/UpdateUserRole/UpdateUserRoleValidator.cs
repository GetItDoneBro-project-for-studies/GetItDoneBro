using FluentValidation;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Commands.UpdateUserRole;

public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleValidator()
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