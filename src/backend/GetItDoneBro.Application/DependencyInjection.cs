using System.Reflection;
using FluentValidation;
using GetItDoneBro.Application.Features.Projects.Commands.CreateProject;
using GetItDoneBro.Application.Features.Projects.Commands.DeleteProject;
using GetItDoneBro.Application.Features.Projects.Commands.UpdateProject;
using GetItDoneBro.Application.Features.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.Features.Projects.Queries.GetProjectById;
using Microsoft.Extensions.DependencyInjection;

namespace GetItDoneBro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ICreateProjectHandler, CreateProjectHandler>();
        services.AddScoped<IUpdateProjectHandler, UpdateProjectHandler>();
        services.AddScoped<IDeleteProjectHandler, DeleteProjectHandler>();

        services.AddScoped<IGetProjectByIdHandler, GetProjectByIdHandler>();
        services.AddScoped<IGetAllProjectsHandler, GetAllProjectsHandler>();

        return services;
    }
}
