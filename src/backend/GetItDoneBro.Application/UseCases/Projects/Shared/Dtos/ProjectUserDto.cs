using GetItDoneBro.Domain.Enums;

namespace GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;

public record ProjectUserDto(
    Guid Id,
    string KeycloakId,
    ProjectRole Role,
    DateTime AssignedAtUtc
);