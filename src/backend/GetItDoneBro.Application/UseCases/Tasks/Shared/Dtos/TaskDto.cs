namespace GetItDoneBro.Application.UseCases.Tasks.Shared.Dtos;

public record TaskDto(
    Guid Id,
    Guid ProjectId,
    Guid TaskColumnId,
    string Title,
    string Description,
    string? AssignedToKeycloakId,
    Uri? ImageUrl,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
