namespace GetItDoneBro.Application.UseCases.Tasks.Shared.Routes;

public static class RouteConsts
{
    public const string BaseRoute = "/api/v1/tasks";
    public const string ByProject = "/api/v1/projects/{projectId:guid}/tasks";
    public const string ByColumn = "/api/v1/task-columns/{columnId:guid}/tasks";
    public const string ById = "/api/v1/tasks/{id:guid}";
    public const string MoveToColumn = "/api/v1/tasks/{id:guid}/move";
}
