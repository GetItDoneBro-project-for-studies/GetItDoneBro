using GetItDoneBro.Domain.Enums;

namespace GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;

public record ProjectUserDto(
    Guid Id,
    string Username,
    string? FirstName,
    string? LastName,
    ProjectRole Role,
    DateTime AssignedAtUtc
);
