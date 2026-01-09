namespace GetItDoneBro.Application.UseCases.Projects.Shared.Routes;

internal static class RouteConsts
{
    internal const string BaseRoute = "/api/v1/projects";
    internal const string ByIdRoute = $"{BaseRoute}/{{id:guid}}";
    internal const string UsersRoute = $"{BaseRoute}/{{projectId:guid}}/users";
    internal const string UserByIdRoute = $"{BaseRoute}/{{projectId:guid}}/users/{{userId:guid}}";
}
