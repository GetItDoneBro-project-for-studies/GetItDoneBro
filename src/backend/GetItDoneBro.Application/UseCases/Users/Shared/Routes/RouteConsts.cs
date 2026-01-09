namespace GetItDoneBro.Application.UseCases.Users.Shared.Routes;

internal static class RouteConsts
{
    internal const string BaseRoute = "/api/v1/users";
    internal const string DisableRoute = $"{BaseRoute}/{{id:guid}}/disable";
    internal const string ResetPasswordRoute = $"{BaseRoute}/{{id:guid}}/reset-password";
}
