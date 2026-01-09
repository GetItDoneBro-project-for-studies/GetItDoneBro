namespace GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Routes;

internal static class RouteConsts
{
    internal const string ProjectUsersRoute = "/api/v1/projects/{projectId:guid}/users";
    internal const string ProjectUserByKeycloakIdRoute = "/api/v1/projects/{projectId:guid}/users/{keycloakId}";
    internal const string CurrentUserProjectsRoute = "/api/v1/users/me/projects";
}