using GetItDoneBro.Domain.Enums;

namespace GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;

public record UserProjectDto(
    Guid ProjectId,
    string ProjectName,
    string ProjectDescription,
    ProjectRole Role,
    DateTime AssignedAtUtc
);
