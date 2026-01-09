namespace GetItDoneBro.Application.UseCases.TaskColumns.Shared.Dtos;

public record TaskColumnDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    int OrderIndex,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
