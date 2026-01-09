namespace GetItDoneBro.Application.UseCases.TaskColumns.Shared.Routes;

public static class RouteConsts
{
    public const string BaseRoute = "/api/v1/task-columns";
    public const string ByProject = "/api/v1/projects/{projectId:guid}/task-columns";
    public const string ById = "/api/v1/task-columns/{id:guid}";
}
